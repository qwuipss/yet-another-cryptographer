using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using AegisCryptographer.Helpers;
using AegisCryptographer.Tests.NotTests.Extensions;
using FluentAssertions;

namespace AegisCryptographer.Tests.Helpers;

public class RegexHelper_Tests
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.ExtractQuotesStringWithEscapedQuotes))]
    public void ExtractQuotesStringWithEscapedQuotes_should_extract_parameter_string_in_double_quotes(
        (string Data, IEnumerable<string> ExpectedStrings) bundle)
    {
        var quotesStrings = RegexHelper.ExtractQuotesStringWithEscapedQuotes(bundle.Data);

        quotesStrings.Should().BeEquivalentTo(bundle.ExpectedStrings);
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.ExtractFlags))]
    public void ExtractFlags_should_extract_arguments_string_and_flags(
        (string Data, string ExpectedArgumentsString, IEnumerable<(string Flag, string Value)> ExpectedFlags) bundle)
    {
        var (argumentsString, flags) = RegexHelper.ExtractFlags(bundle.Data);

        argumentsString.Should().BeEquivalentTo(bundle.ExpectedArgumentsString);
        flags.Should().BeEquivalentTo(bundle.ExpectedFlags);
    }

    private static class TestCases
    {
        public static IEnumerable<(string Data, IEnumerable<string> Expected)> ExtractQuotesStringWithEscapedQuotes
        {
            get
            {
                yield return ($"encrypt string {string.Empty.WrapInQuotes()}", [string.Empty]);
                yield return ($"encrypt string {"  with   spaces   ".WrapInQuotes()}", ["  with   spaces   "]);
                yield return ($"encrypt string {"text after".WrapInQuotes()} -a aesgcm", ["text after"]);
                yield return ($"encrypt string {"simple".WrapInQuotes()} random text", ["simple"]);
                yield return ($"encrypt string {"any symb тест 123 \\:;!@%()[]{}=".WrapInQuotes()}", ["any symb тест 123 \\:;!@%()[]{}="]);
                yield return ($"encrypt string {"two words".WrapInQuotes()}", ["two words"]);
                yield return ($"encrypt string {"special \t symbols\r".WrapInQuotes()}", ["special \t symbols\r"]);
                yield return ($"decrypt string {$"with escaped {"quotes".WrapInEscapedQuotes()}".WrapInQuotes()}",
                    [$"with escaped {"quotes".WrapInQuotes()}"]);
                yield return (
                    $"decrypt string {$"with many escaped {string.Empty.WrapInEscapedQuotes()} {"quotes".WrapInEscapedQuotes()}".WrapInQuotes()}",
                    [$"with many escaped {string.Empty.WrapInQuotes()} {"quotes".WrapInQuotes()}"]);
                yield return (
                    $"decrypt string {$"with many escaped {string.Empty.WrapInEscapedQuotes()} {"quotes".WrapInEscapedQuotes()}".WrapInQuotes()} {$"another {"escaped".WrapInEscapedQuotes()}".WrapInQuotes()}",
                    [
                        $"with many escaped {string.Empty.WrapInQuotes()} {"quotes".WrapInQuotes()}",
                        $"another {"escaped".WrapInQuotes()}"
                    ]);
                yield return (
                    $"{$"escaped {"before".WrapInEscapedQuotes()}".WrapInQuotes()} encrypt string {"another escaped".WrapInQuotes()}",
                    [$"escaped {"before".WrapInQuotes()}", "another escaped"]);
            }
        }

        public static IEnumerable<(string Data, string ExpectedArgumentsString, IEnumerable<(string Flag, string Value)>
                ExpectedFlags)>
            ExtractFlags
        {
            get
            {
                yield return ($"encrypt string {"hello".WrapInQuotes()} -flag simple",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-flag", "simple")]);
                yield return (
                    $"{"--no extract --before".WrapInQuotes()} encrypt string {"hello".WrapInQuotes()} -flag simple",
                    $"{"--no extract --before".WrapInQuotes()} encrypt string {"hello".WrapInQuotes()}",
                    [("-flag", "simple")]);
                yield return (
                    $"{"--no extract --before".WrapInQuotes()} encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()} -flag simple",
                    $"{"--no extract --before".WrapInQuotes()} encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()}",
                    [("-flag", "simple")]);
                yield return ($"encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()} -flag simple",
                    $"encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()}",
                    [("-flag", "simple")]);
                yield return (
                    $"encrypt string {"--no-extract first".WrapInQuotes()} -flag simple {"--no-extract second".WrapInQuotes()}",
                    $"encrypt string {"--no-extract first".WrapInQuotes()} {"--no-extract second".WrapInQuotes()}",
                    [("-flag", "simple")]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with many --flags",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-with", "many"), ("--flags", string.Empty)]);
                yield return (
                    $"encrypt string {$"--no-extract with -escaped {"quotes".WrapInEscapedQuotes()}".WrapInQuotes()} -with many --flags",
                    $"encrypt string {$"--no-extract with -escaped {"quotes".WrapInEscapedQuotes()}".WrapInQuotes()}",
                    [("-with", "many"), ("--flags", string.Empty)]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with two --flags",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-with", "two"), ("--flags", string.Empty)]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with two --flags with-value",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-with", "two"), ("--flags", "with-value")]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with two -simple flags",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-with", "two"), ("-simple", "flags")]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -short {"wrapped".WrapInQuotes()}",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-short", "wrapped".WrapInQuotes())]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} --long {"wrapped".WrapInQuotes()}",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("--long", "wrapped".WrapInQuotes())]);
                yield return (
                    $"encrypt string {"hello".WrapInQuotes()} -short {"wrapped  and . dots .".WrapInQuotes()}",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-short", "wrapped  and . dots .".WrapInQuotes())]);
                yield return (
                    $"encrypt string {"hello".WrapInQuotes()} --long {"wrapped  and . dots .".WrapInQuotes()}",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("--long", "wrapped  and . dots .".WrapInQuotes())]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} --with {"not complete long".WrapInQuotes()} --",
                    $"encrypt string {"hello".WrapInQuotes()} --",
                    [("--with", "not complete long".WrapInQuotes())]);
                yield return (
                    $"encrypt string {"hello".WrapInQuotes()} -with {"not complete short".WrapInQuotes()} - -",
                    $"encrypt string {"hello".WrapInQuotes()} - -",
                    [("-with", "not complete short".WrapInQuotes())]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} --two-no --value-flags",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("--two-no", string.Empty), ("--value-flags", string.Empty)]);
                yield return (
                    $"encrypt -inside {"args".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-inside", "args".WrapInQuotes()), ("--long-no-value", string.Empty)]);
                yield return (
                    $"encrypt -inside {"any word symb тест 123".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-inside", "any word symb тест 123".WrapInQuotes()), ("--long-no-value", string.Empty)]);
                yield return (
                    $"encrypt -inside {"args  and dots ...".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-inside", "args  and dots ...".WrapInQuotes()), ("--long-no-value", string.Empty)]);
                yield return (
                    $"encrypt --inside-args {"wrapped  and . dots ...".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("--inside-args", "wrapped  and . dots ...".WrapInQuotes()), ("--long-no-value", string.Empty)]);
                yield return (
                    $"encrypt --inside-args {"any word symb тест".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-with-value 256",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("--inside-args", "any word symb тест".WrapInQuotes()), ("--long-with-value", "256")]);
                yield return (
                    $"encrypt --inside-args {"with digits 123".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-with-value 256asd",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("--inside-args", "with digits 123".WrapInQuotes()), ("--long-with-value", "256asd")]);
                yield return (
                    $"encrypt -inside {"with digits 123".WrapInQuotes()} string {"hello".WrapInQuotes()} -shortValue 256",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-inside", "with digits 123".WrapInQuotes()), ("-shortValue", "256")]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -caSEsenSetiVE 123asd",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-caSEsenSetiVE", "123asd")]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -out {"C://path".WrapInQuotes()}",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-out", "C://path".WrapInQuotes())]);
                yield return ($"encrypt string {"hello".WrapInQuotes()} -out {"C:\\path/".WrapInQuotes()}",
                    $"encrypt string {"hello".WrapInQuotes()}",
                    [("-out", "C:\\path/".WrapInQuotes())]);
            }
        }
    }
}