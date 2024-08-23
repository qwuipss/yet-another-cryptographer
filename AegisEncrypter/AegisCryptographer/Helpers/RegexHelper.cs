using System.Text.RegularExpressions;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;

namespace AegisCryptographer.Helpers;

public static partial class RegexHelper
{
    [GeneratedRegex("[a-zA-Z0-9\\-]+")]
    public static partial Regex GetCommandFlagDefaultValueValidationRegex();

    public static string? GetQuotesStringWithEscapedQuotes(string str)
    {
        return QuotesStringWithEscapedQuotesRegex().Matches(str)
            .Select(y => GetGroupValue(y, "value").Replace("\\\"", "\"")).SingleOrDefault();
    }

    public static (string ArgumentsString, IEnumerable<(string Flag, string Value)> Flags)
        SplitExecutionStringInfo(string str)
    {
        var (argumentsString, flagsMatchList) = GetCommandArgumentsString(str);

        return (argumentsString, GetCommandFlags(flagsMatchList));
    }

    private static (string ArgumentsString, List<Match> FlagsMatchList) GetCommandArgumentsString(string str)
    {
        var notFlagValueStringsInfo = new List<(string MatchString, int Index)>();

        var clearedString = Regex.Replace(str, NotFlagValueQuotesStringWithEscapedQuotes().ToString(), match =>
        {
            var matchString = match.ToString();
            notFlagValueStringsInfo.Add((matchString, match.Index));
            return new string(' ', matchString.Length);
        });

        var flagsMatchList = new List<Match>();

        var defectiveArgumentsString = Regex.Replace(clearedString, FlagsRegex().ToString(), match =>
        {
            flagsMatchList.Add(match);
            return new string(' ', match.ToString().Length);
        });

        var argumentsString = defectiveArgumentsString;

        notFlagValueStringsInfo.ForEach(bundle =>
            argumentsString = argumentsString.ReplaceWithOverWriting(bundle.MatchString, bundle.Index));

        argumentsString = Regex.Replace(argumentsString, WhiteSpacesRegex().ToString(), " ").Trim();

        return (argumentsString, flagsMatchList);
    }

    private static IEnumerable<(string Flag, string Value)> GetCommandFlags(IEnumerable<Match> matchCollection)
    {
        return matchCollection.Select(match =>
        {
            string key, value;

            if ((key = GetGroupValue(match, "shortKey")).Length is not 0)
                value = GetGroupValue(match, "shortValue");
            else if ((key = GetGroupValue(match, "longKey")).Length is not 0)
                value = GetGroupValue(match, "longValue");
            else if ((key = GetGroupValue(match, "longKeyNoValue")).Length is not 0)
                value = string.Empty;
            else
                throw new InternalException("Unexpected code block entering.");

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

    [GeneratedRegex(
        "(?<!(?:(?:-[a-zA-Z]+)|(?:--(?:[a-zA-Z][a-zA-Z\\-]*)))\\s+)(?<=(?:^|\\s))\"(?<value>[^\"\"\\\\]|\\\\.)*\"(?=(?:$|\\s))")]
    private static partial Regex NotFlagValueQuotesStringWithEscapedQuotes();

    [GeneratedRegex(
        """(?:(?<=^|\s)(?<shortKey>-[a-zA-Z]+)\s+(?<shortValue>(?:[a-zA-Z0-9][a-zA-Z0-9\-]*)|(?:"(?:[\w\s\.\\\\/:])*"))(?=$|\s))|(?:(?<=^|\s)(?<longKey>--(?:[a-zA-Z][[a-zA-Z\-]*))\s+(?<longValue>(?:[a-zA-Z0-9][a-zA-Z0-9\-]*)|(?:"(?:[\w\s\.\\\\/:])*"))(?=$|\s))|(?:(?<=^|\s)(?<longKeyNoValue>--(?:[a-zA-Z][a-zA-Z\-]*))(?=$|\s))""")]
    private static partial Regex FlagsRegex();
}