using System.Text;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using AegisCryptographer.Helpers;
using AegisCryptographer.IO;
using static AegisCryptographer.Commands.CommandsTokens;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;

namespace AegisCryptographer.Commands.Resolvers;

public class CommandResolver(
    string? input,
    ICommandFlagsResolver flagsResolver,
    IReader reader,
    IWriter writer) : ICommandResolver
{
    private IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;

    public ICommand Resolve()
    {
        if (StringExtensions.IsNullOrEmptyOrWhitespace(input)) throw new InputEmptyException();

        var (argumentsString, flagsBundles) = RegexHelper.SplitExecutionStringInfo(input!);
        var commandExecutionStringInfo =
            new CommandExecutionStringInfo(SplitCommandArguments(argumentsString), ResolveCommandFlags(flagsBundles, flagsResolver));

        var commandToExecuteToken = commandExecutionStringInfo.CommandArgumentsCollection.Next(CommandToExecute);

        return commandToExecuteToken switch
        {
            EncryptLongToken or EncryptShortToken => new EncryptCommandResolver(commandExecutionStringInfo, Reader, Writer).Resolve(),
            DecryptLongToken or DecryptShortToken => new DecryptCommandResolver(commandExecutionStringInfo, Reader, Writer).Resolve(),
            _ => throw new CommandResolveException(commandToExecuteToken)
        };
    }

    private static CommandArgumentsCollection SplitCommandArguments(string commandExecutionString)
    {
        var stringBuilder = new StringBuilder();
        var argumentsCollection = new List<string>();

        for (var i = 0; i < commandExecutionString.Length; i++)
            if (commandExecutionString[i] is '"')
            {
                while (i < commandExecutionString.Length - 1)
                {
                    stringBuilder.Append(commandExecutionString[i]);
                    i++;

                    if (commandExecutionString[i] is not '"' || commandExecutionString[i - 1] is '\\') continue;

                    stringBuilder.Append(commandExecutionString[i]);
                    argumentsCollection.Add(stringBuilder.ToString());
                    stringBuilder.Clear();

                    break;
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

        if (stringBuilder.Length is not 0) argumentsCollection.Add(stringBuilder.ToString());

        return new CommandArgumentsCollection(argumentsCollection);
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
}