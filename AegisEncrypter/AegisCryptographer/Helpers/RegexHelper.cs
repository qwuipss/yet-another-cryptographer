using System.Text.RegularExpressions;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Helpers;

public static partial class RegexHelper
{
    [GeneratedRegex("[a-zA-Z0-9\\-]+")]
    public static partial Regex GetCommandFlagDefaultValueValidationRegex();

    public static string? ExtractArgumentString(string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        var matchCollection = StringArgumentWithEscapedQuotesRegex().Matches(str);

        return matchCollection.Count switch
        {
            0 => null,
            
        };
    }

    public static (string ArgumentsString, CommandFlagsCollection Flags) ExtractExecuteStringFlags(string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        var matchCollection = new List<Match>();

        var argumentsString = Regex.Replace(str, ExecuteStringFlagsRegex().ToString(), match =>
        {
            matchCollection.Add(match);
            return string.Empty;
        });

        argumentsString = Regex.Replace(argumentsString, WhiteSpacesRegex().ToString(), " ").Trim();
        
        return (argumentsString, GetCommandFlagsCollection(matchCollection));
    }

    private static CommandFlagsCollection GetCommandFlagsCollection(IReadOnlyCollection<Match> matchCollection)
    {
        var commandFlags = new HashSet<ICommandFlag>();

        if (matchCollection.Count is 0) return new CommandFlagsCollection(commandFlags);

        foreach (var match in matchCollection)
        {
            string key, value;

            if ((key = GetGroupValue(match, "singleKey")).Length is not 0)
            {
                value = GetGroupValue(match, "singleValue");
            }
            else if ((key = GetGroupValue(match, "doubleKey")).Length is not 0)
            {
                value = GetGroupValue(match, "doubleValue");
            }
            else if ((key = GetGroupValue(match, "doubleKeyNoValue")).Length is not 0)
            {
                value = string.Empty;
            }
            else
            {
                throw new InternalException("Unexpected code block entering.");
            }

            var resolvedFlag = BaseCommandFlag.ResolveCommandFlag(key, value);

            if (commandFlags.TryGetValue(resolvedFlag, out var existedFlag))
                throw new FlagDuplicateException(resolvedFlag.Key, existedFlag.Key);

            commandFlags.Add(resolvedFlag);
        }

        return new CommandFlagsCollection(commandFlags);
    }

    private static string GetGroupValue(Match match, string groupName)
    {
        return match.Groups[groupName].Value;
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex WhiteSpacesRegex();
    
    [GeneratedRegex("\"(?<value>[^\"\\\\]|\\\\.)*\"")]
    private static partial Regex StringArgumentWithEscapedQuotesRegex();

    [GeneratedRegex(
        "(?:(?<=^|\\s)(?<singleKey>-\\p{L}+)\\s+(?<singleValue>\\w[\\w\\-]*)(?=$|\\s))|(?:(?<=^|\\s)(?<singleKey>-\\p{L}+)\\s+(?<singleValue>\\\"\\w[\\w\\-]*\\\")(?=$|\\s))|(?:(?<=^|\\s)(?<doubleKey>--\\p{L}+)\\s+(?<doubleValue>\\w[\\w\\-]*)(?=$|\\s))|(?:(?<=^|\\s)(?<doubleKey>--\\p{L}+)\\s+(?<doubleValue>\\\"\\w[\\w\\-]*\\\")(?=$|\\s))|(?:(?<=^|\\s)(?<doubleKeyNoValue>--\\p{L}+)(?=$|\\s))")]
    private static partial Regex ExecuteStringFlagsRegex();
}