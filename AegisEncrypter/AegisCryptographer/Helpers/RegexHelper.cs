using System.Text.RegularExpressions;
using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Helpers;

public static partial class RegexHelper
{
    public static string? ExtractParameterString(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);
        
        var matchCollection = ParameterStringRegex().Matches(str);

        return matchCollection.Count switch
        {
            0 => null,
            > 1 => throw new AmbiguousArgumentException(),
            _ => matchCollection.First().Value[1..^1].Replace("\\\"", "\"")
        };
    }

    [GeneratedRegex("\"((?:[^\"\\\\]|\\\\.)*)\"")]
    private static partial Regex ParameterStringRegex();
}