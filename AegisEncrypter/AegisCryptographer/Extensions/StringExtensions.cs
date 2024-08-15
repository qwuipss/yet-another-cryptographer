using System.Collections.Immutable;
using System.Text;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Helpers;

namespace AegisCryptographer.Extensions;

public static class StringExtensions
{
    private static readonly ImmutableArray<int> DefaultCipherKeyPaddings = [128 / 8, 192 / 8, 256 / 8];

    public static bool IsNullOrEmptyOrWhitespace(string? str)
    {
        return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }

    public static string ReplaceWithOverWriting(this string str, string replaceString, int index)
    {
        return str.Remove(index, replaceString.Length).Insert(index, replaceString);
    }

    public static string WrapInQuotes(this string str)
    {
        return $"\"{str}\"";
    }

    public static ReadOnlySpan<byte> ToPaddedSecretKey(this string str, ImmutableArray<int>? paddings = null)
    {
        paddings ??= DefaultCipherKeyPaddings;
        var size = Settings.Encoding.GetByteCount(str);
        var paddedSize = paddings.Value.FirstOrDefault(x => x >= size);

        if (paddedSize is 0) throw new SecretTooLongException(size, paddings.Max());

        var array = new byte[paddedSize];
        var bytes = Settings.Encoding.GetBytes(str);

        Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

        return new ReadOnlySpan<byte>(array);
    }

    public static CommandExecutionStringInfo ToCommandExecutionStringInfo(this string str,
        ICommandFlagsResolver flagsResolver)
    {
        var (argumentsString, flagsBundles) = RegexHelper.ExtractFlags(str);

        return new CommandExecutionStringInfo(SplitCommandArguments(argumentsString),
            ResolveCommandFlags(flagsBundles, flagsResolver));
    }

    private static CommandFlagsCollection ResolveCommandFlags(IEnumerable<(string Flag, string Value)> flagsBundles,
        ICommandFlagsResolver flagsResolver)
    {
        var flagsCollection = new CommandFlagsCollection();

        flagsBundles.ForEach(flagBundle =>
        {
            var resolvedFlag = flagsResolver.Resolve(flagBundle.Flag, flagBundle.Value);

            if (flagsCollection.TryGetValue(resolvedFlag, out var existedFlag))
                throw new FlagDuplicateException(resolvedFlag.Key, existedFlag.Key);
        });

        return flagsCollection;
    }

    private static CommandArgumentsCollection SplitCommandArguments(string commandExecutionString)
    {
        var stringBuilder = new StringBuilder();
        var argumentsCollection = new List<string>();

        for (int i = 0; i < commandExecutionString.Length; i++)
        {
            if (commandExecutionString[i] is '"')
            {
                while (true)
                {
                    stringBuilder.Append(commandExecutionString[i]);
                    i++;

                    if (commandExecutionString[i] is '"' && commandExecutionString[i - 1] is not '\\')
                    {
                        break;
                    }
                }
            }
            else
            {
                if (char.IsWhiteSpace(commandExecutionString[i]))
                {
                    if (stringBuilder.Length is not 0)
                    {
                        argumentsCollection.Add(stringBuilder.ToString());
                        stringBuilder.Clear();
                    }

                    continue;
                }

                stringBuilder.Append(commandExecutionString[i]);
            }
        }

        return new CommandArgumentsCollection(argumentsCollection);
    }
}