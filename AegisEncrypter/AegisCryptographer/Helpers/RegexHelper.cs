using System.Text.RegularExpressions;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Helpers;

public static partial class RegexHelper
{
    public static string? ExtractArgumentString(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        var matchCollection = ArgumentStringRegex().Matches(str);

        return matchCollection.Count switch
        {
            0 => null,
            > 1 => throw new AmbiguousArgumentException(),
            _ => matchCollection[0].Groups.Values.First(x => x.Name == "value").Value.Replace("\\\"", "\"")
        };
    }

    public static (string CutString, CommandFlagsCollection Flags) ExtractExecuteStringFlags(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        var matchCollection = ExecuteStringFlagsRegex().Matches(str);

        return matchCollection.Count switch
        {
            0 => (str, GetCommandFlagsCollection(matchCollection)),
            _ => (str[..matchCollection[0].Index].Trim(), GetCommandFlagsCollection(matchCollection))
        };
    }

    private static CommandFlagsCollection GetCommandFlagsCollection(MatchCollection matchCollection)
    {
        var commandFlags = new HashSet<ICommandFlag>();

        if (matchCollection.Count is 0) return new CommandFlagsCollection(commandFlags);

        for (var i = 0; i < matchCollection.Count; i++)
        {
            var match = matchCollection[i];

            string key, value;

            if (GetGroupValue(match, "single").Length is not 0)
            {
                key = GetGroupValue(matchCollection[i], "singleKey");
                value = GetGroupValue(matchCollection[i], "singleValue");
            }
            else if (GetGroupValue(match, "double").Length is not 0)
            {
                key = GetGroupValue(matchCollection[i], "doubleKey");
                value = GetGroupValue(matchCollection[i], "doubleValue");
            }
            else
            {
                throw new InternalException("Unexpected code block entering.");
            }

            var resolvedFlag = BaseCommandFlag.ResolveCommandFlag(key, value);

            if (commandFlags.TryGetValue(resolvedFlag, out var flag))
                throw new FlagDuplicateException(resolvedFlag.Key, flag.Key);

            commandFlags.Add(resolvedFlag);
        }

        return new CommandFlagsCollection(commandFlags);
    }

    [GeneratedRegex("[a-zA-Z0-9\\-]*")]
    public static partial Regex GetCommandFlagDefaultValueValidationRegex();

    private static string GetGroupValue(Match match, string groupName)
    {
        return match.Groups[groupName].Value;
    }

    [GeneratedRegex("\"(?<value>(?:[^\"\\\\]|\\\\.)*)\"")]
    private static partial Regex ArgumentStringRegex();

    [GeneratedRegex(
        "(?<!\"|-|--)(?<single>(?<singleKey>-[a-zA-Z]+) +(?<singleValue>[a-zA-Z0-9]+)(?![^\"]*\"))|(?<double>(?<doubleKey>--[a-zA-Z\\-]+) *(?<doubleValue>[a-zA-Z0-9]*)(?![^\"]*\"))")]
    private static partial Regex ExecuteStringFlagsRegex();
}