using System.Text.RegularExpressions;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Resolvers;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;

namespace AegisCryptographer.Services;

public partial class RegexService : IRegexService
{
    [GeneratedRegex("[a-zA-Z0-9\\-]+")]
    public static partial Regex GetCommandFlagDefaultValueValidationRegex();

    public string? GetQuotesStringWithEscapedQuotes(string str)
    {
        return QuotesStringWithEscapedQuotesRegex()
            .Matches(str)
            .Select(match => GetGroupValue(match, "value").Replace("\\\"", "\""))
            .SingleOrDefault();
    }    
    
    public IEnumerable<string> SplitCommandArgumentsString(string str)
    {
        return CommandArgumentsString()
            .Matches(str)
            .Select(match => match.ToString().Replace("\\\"", "\"")); //todo
    }

    public ISplitExecutionStringInfo SplitExecutionStringInfo(string str)
    {
        var (arguments, flagsMatches) = GetCommandArgumentsAndFlagsMatches(str);
        var flags = GetCommandFlags(flagsMatches);
        
        return new SplitExecutionStringInfo(arguments, flags);
    }

    private static (string Arguments, List<Match> FlagsMatches) GetCommandArgumentsAndFlagsMatches(string str)
    {
        var notFlagValueStringsInfo = new List<(string MatchString, int Index)>();

        var clearedString = Regex.Replace(str, NotFlagValueQuotesStringWithEscapedQuotes().ToString(), match =>
        {
            var matchString = match.ToString();
            notFlagValueStringsInfo.Add((matchString, match.Index));
            return new string(' ', matchString.Length);
        });

        var flagsMatchList = new List<Match>();

        var defectiveArguments = Regex.Replace(clearedString, FlagsRegex().ToString(), match =>
        {
            flagsMatchList.Add(match);
            return new string(' ', match.ToString().Length);
        });

        var arguments = defectiveArguments;

        notFlagValueStringsInfo.ForEach(bundle =>
            arguments = arguments.ReplaceWithOverWriting(bundle.MatchString, bundle.Index));

        arguments = Regex.Replace(arguments, WhiteSpacesRegex().ToString(), " ").Trim();

        return (arguments, flagsMatchList);
    }

    private static IEnumerable<(string Flag, string Value)> GetCommandFlags(IEnumerable<Match> matchCollection)
    {
        return matchCollection.Select(match =>
        {
            string key, value;

            if ((key = GetGroupValue(match, "shortKey")).Length is not 0) value = GetGroupValue(match, "shortValue");
            else if ((key = GetGroupValue(match, "longKey")).Length is not 0) value = GetGroupValue(match, "longValue");
            else if ((key = GetGroupValue(match, "longKeyNoValue")).Length is not 0) value = string.Empty;
            else throw new InternalException("Unexpected code block entering.");

            return (key, value);
        });
    }

    private static string GetGroupValue(Match match, string groupName)
    {
        return match.Groups[groupName].Value;
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex WhiteSpacesRegex();

    [GeneratedRegex("\"(?<value>([^\"\\\\]|\\\\.)*)\"")]
    private static partial Regex QuotesStringWithEscapedQuotesRegex();
    
    [GeneratedRegex("(\"([^\"\\\\]|\\\\.)*\")|([^ ]+)")]
    private static partial Regex CommandArgumentsString();

    [GeneratedRegex(
        "(?<!(?:(?:-[a-zA-Z]+)|(?:--(?:[a-zA-Z][a-zA-Z\\-]*)))\\s+)(?<=(?:^|\\s))\"(?<value>[^\"\"\\\\]|\\\\.)*\"(?=(?:$|\\s))")]
    private static partial Regex NotFlagValueQuotesStringWithEscapedQuotes();

    [GeneratedRegex(
        """(?:(?<=^|\s)(?<shortKey>-[a-zA-Z]+)\s+(?<shortValue>(?:[a-zA-Z0-9][a-zA-Z0-9\-]*)|(?:"(?:[\w\s\.\\\\/:])*"))(?=$|\s))|(?:(?<=^|\s)(?<longKey>--(?:[a-zA-Z][[a-zA-Z\-]*))\s+(?<longValue>(?:[a-zA-Z0-9][a-zA-Z0-9\-]*)|(?:"(?:[\w\s\.\\\\/:])*"))(?=$|\s))|(?:(?<=^|\s)(?<longKeyNoValue>--(?:[a-zA-Z][a-zA-Z\-]*))(?=$|\s))""")]
    private static partial Regex FlagsRegex();
}