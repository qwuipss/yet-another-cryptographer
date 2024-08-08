using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;
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

    [TestCaseSource(typeof(TestCases), nameof(TestCases.ExtractExecuteStringFlags))]
    public void ExtractExecuteStringFlags_should_extract_arguments_string_and_flags(
        (string Data, string ExpectedArgumentsString, CommandFlagsCollection ExpectedFlags) bundle)
    {
        var (argumentsString, flags) = RegexHelper.ExtractExecuteStringFlags(bundle.Data);

        argumentsString.Should().BeEquivalentTo(bundle.ExpectedArgumentsString);
        flags.Should().BeEquivalentTo(bundle.ExpectedFlags);
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.ExtractExecuteStringFlagsThrows))]
    public void ExtractExecuteStringFlags_should_throw_exceptions(
        (string Data, Exception ExpectedException) bundle)
    {
        Assert.Throws(bundle.ExpectedException.GetType(), () => { _ = RegexHelper.ExtractExecuteStringFlags(bundle.Data); });
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

        public static IEnumerable<(string Data, string ExpectedArgumentsString, CommandFlagsCollection ExpectedFlags)>
            ExtractExecuteStringFlags
        {
            get
            {
                yield return ($"encrypt string {WrapInQuotes("hello")} -alg aesgcm",
                    $"encrypt string {WrapInQuotes("hello")}",
                    new CommandFlagsCollection([new AlgorithmCommandFlag("-alg", "aesgcm")]));
                yield return ($"encrypt string {WrapInQuotes("world")} --algorithm blowfish",
                    $"encrypt string {WrapInQuotes("world")}",
                    new CommandFlagsCollection([new AlgorithmCommandFlag("--algorithm", "blowfish")]));
                yield return ($"--algorithm aesgcm encrypt string {WrapInQuotes("world")}",
                    $"encrypt string {WrapInQuotes("world")}",
                    new CommandFlagsCollection([new AlgorithmCommandFlag("--algorithm", "aesgcm")]));
                yield return ($"encrypt -alg sha256 string {WrapInQuotes("world")}",
                    $"encrypt string {WrapInQuotes("world")}",
                    new CommandFlagsCollection([new AlgorithmCommandFlag("-alg", "sha256")]));
            }
        }

        public static IEnumerable<(string Data, Exception Exception)>
            ExtractExecuteStringFlagsThrows
        {
            get
            {
                yield return ($"encrypt string {WrapInQuotes("hello")} -alg aesgcm -alg aesgcm",
                    new FlagDuplicateException("-alg", "-alg"));
                yield return ($"encrypt string {WrapInQuotes("world")} --algorithm blowfish -alg aesgcm",
                    new FlagDuplicateException("-alg", "--algorithm"));
                yield return ($"-alg aesgcm encrypt --algorithm tdes string {WrapInQuotes("world")}",
                    new FlagDuplicateException("--algorithm", "-alg"));
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