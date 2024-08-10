namespace AegisCryptographer.Tests.NotTests.Extensions;

public static class StringExtensions
{
    public static string WrapInEscapedQuotes(this string str)
    {
        return $"\\\"{str}\\\"";
    }
}