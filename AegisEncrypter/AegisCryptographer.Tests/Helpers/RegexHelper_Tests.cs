using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Helpers;
using FluentAssertions;

namespace AegisCryptographer.Tests.Helpers;

public class RegexHelper_Tests
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.ArgumentStringInDoubleQuotes))]
    public void ExtractArgumentString_should_extract_parameter_string_in_double_quotes(
        (string Data, string Expected) bundle)
    {
        var str = RegexHelper.ExtractArgumentString(bundle.Data);

        str.Should().BeEquivalentTo(bundle.Expected);
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.ExecuteStringFlags))]
    public void ExtractExecuteStringFlags_should_extract_flags(
        (string Data, string ExpectedCutString, CommandFlagsCollection ExpectedFlags) bundle)
    {
        var (cutString, flags) = RegexHelper.ExtractExecuteStringFlags(bundle.Data);

        cutString.Should().BeEquivalentTo(bundle.ExpectedCutString);
        flags.Should().BeEquivalentTo(bundle.ExpectedFlags);
    }

    private static class TestCases
    {
        public static IEnumerable<(string Data, string Expected)> ArgumentStringInDoubleQuotes
        {
            get
            {
                yield return ($"encrypt string {WrapInQuotes(string.Empty)}", string.Empty);
                yield return ($"encrypt string {WrapInQuotes("  with   spaces   ")}", "  with   spaces   ");
                yield return ($"encrypt string {WrapInQuotes("text after")} -a aesgcm", "text after");
                yield return ($"encrypt string {WrapInQuotes("simple")}", "simple");
                yield return ($"encrypt string {WrapInQuotes("two words")}", "two words");
                yield return ($"decrypt string {WrapInQuotes($"with escaped {WrapInEscapedQuotes("quotes")}")}",
                    $"with escaped {WrapInQuotes("quotes")}");
                yield return (
                    $"decrypt string {WrapInQuotes($"with many escaped {WrapInEscapedQuotes(string.Empty)} {WrapInEscapedQuotes("quotes")}")}",
                    $"with many escaped {WrapInQuotes(string.Empty)} {WrapInQuotes("quotes")}");
            }
        }

        public static IEnumerable<(string Data, string ExpectedCutString, CommandFlagsCollection ExpectedFlags)>
            ExecuteStringFlags
        {
            get
            {
                yield return ($"encrypt string {WrapInQuotes("hello")} -alg aesgcm",
                    $"encrypt string {WrapInQuotes("hello")}",
                    new CommandFlagsCollection([new AlgorithmCommandFlag("-alg", "aesgcm")]));
                yield return ($"encrypt string {WrapInQuotes("world")} --algorithm aesgcm",
                    $"encrypt string {WrapInQuotes("world")}",
                    new CommandFlagsCollection([new AlgorithmCommandFlag("--algorithm", "aesgcm")]));
            }
        }

        private static string WrapInQuotes(string str)
        {
            return $"\"{str}\"";
        }

        private static string WrapInEscapedQuotes(string str)
        {
            return $"\\\"{str}\\\"";
        }
    }
}