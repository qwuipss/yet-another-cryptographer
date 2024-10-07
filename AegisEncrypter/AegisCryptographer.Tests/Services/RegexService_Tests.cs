using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Resolvers;
using AegisCryptographer.Extensions;
using AegisCryptographer.Services;
using AegisCryptographer.Tests.NotTests.Extensions;
using FluentAssertions;

namespace AegisCryptographer.Tests.Helpers;

[TestFixture]
public class RegexService_Tests
{
    private RegexService _regexService;

    [SetUp]
    public void SetUp()
    {
        _regexService = new RegexService();
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.GetQuotesStringWithEscapedQuotes))]
    public void GetQuotesStringWithEscapedQuotes_should_extract_string_in_quotes_with_escaped_quotes((string Data, string? ExpectedString) bundle)
    {
        var quotesStrings = _regexService.GetQuotesStringWithEscapedQuotes(bundle.Data);

        quotesStrings.Should().BeEquivalentTo(bundle.ExpectedString);
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.SplitExecutionStringInfo))]
    public void SplitExecutionStringInfo_should_split_string_on_arguments_string_and_flags(
        (string Data, SplitExecutionStringInfo SplitExecutionStringCollection) bundle)
    {
        var splitExecutionStringCollection = _regexService.SplitExecutionStringInfo(bundle.Data);

        splitExecutionStringCollection.Should().BeEquivalentTo(bundle.SplitExecutionStringCollection);
    }

    private static class TestCases
    {
        public static IEnumerable<(string Data, string? ExpectedString)> GetQuotesStringWithEscapedQuotes
        {
            get
            {
                yield return (string.Empty, null);
                yield return (string.Empty.WrapInQuotes(), string.Empty);
                yield return ("simple".WrapInQuotes(), "simple");
                yield return ("  with   spaces   ".WrapInQuotes(), "  with   spaces   ");
                yield return ("any symb тест 123 \\:;!@%()[]{}=".WrapInQuotes(), "any symb тест 123 \\:;!@%()[]{}=");
                yield return ("special \t symbols\r".WrapInQuotes(), "special \t symbols\r");
                yield return ("caSE  SenSEtIve".WrapInQuotes(), "caSE  SenSEtIve");
                yield return ($"with escaped {"quotes".WrapInEscapedQuotes()}".WrapInQuotes(),
                    $"with escaped {"quotes".WrapInQuotes()}");
                yield return (
                    $"with many escaped {string.Empty.WrapInEscapedQuotes()} {"quotes".WrapInEscapedQuotes()}"
                        .WrapInQuotes(),
                    $"with many escaped {string.Empty.WrapInQuotes()} {"quotes".WrapInQuotes()}");
            }
        }

        public static IEnumerable<(string Data, SplitExecutionStringInfo SplitExecutionStringCollection)>
            SplitExecutionStringInfo
        {
            get
            {
                yield return (string.Empty,
                    new SplitExecutionStringInfo(string.Empty,
                        []));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -flag simple",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-flag", "simple")]));
                yield return (
                    $"{"--no extract --before".WrapInQuotes()} encrypt string {"hello".WrapInQuotes()} -flag simple",
                    new SplitExecutionStringInfo($"{"--no extract --before".WrapInQuotes()} encrypt string {"hello".WrapInQuotes()}",
                        [("-flag", "simple")]));
                yield return (
                    $"{"--no extract --before".WrapInQuotes()} encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()} -flag simple",
                    new SplitExecutionStringInfo(
                        $"{"--no extract --before".WrapInQuotes()} encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()}",
                        [("-flag", "simple")]));
                yield return ($"encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()} -flag simple",
                    new SplitExecutionStringInfo($"encrypt string {"--no-extract -flags inside quotes".WrapInQuotes()}",
                        [("-flag", "simple")]));
                yield return (
                    $"encrypt string {"--no-extract first".WrapInQuotes()} -flag simple {"--no-extract second".WrapInQuotes()}",
                    new SplitExecutionStringInfo($"encrypt string {"--no-extract first".WrapInQuotes()} {"--no-extract second".WrapInQuotes()}",
                        [("-flag", "simple")]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with many --flags",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-with", "many"), ("--flags", string.Empty)]));
                yield return (
                    $"encrypt string {$"--no-extract with -escaped {"quotes".WrapInEscapedQuotes()}".WrapInQuotes()} -with many --flags",
                    new SplitExecutionStringInfo(
                        $"encrypt string {$"--no-extract with -escaped {"quotes".WrapInEscapedQuotes()}".WrapInQuotes()}",
                        [("-with", "many"), ("--flags", string.Empty)]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with two --flags",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-with", "two"), ("--flags", string.Empty)]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with two --flags with-value",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-with", "two"), ("--flags", "with-value")]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -with two -simple flags",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-with", "two"), ("-simple", "flags")]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -short {"wrapped".WrapInQuotes()}",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-short", "wrapped".WrapInQuotes())]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} --long {"wrapped".WrapInQuotes()}",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("--long", "wrapped".WrapInQuotes())]));
                yield return (
                    $"encrypt string {"hello".WrapInQuotes()} -short {"wrapped  and . dots .".WrapInQuotes()}",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-short", "wrapped  and . dots .".WrapInQuotes())]));
                yield return (
                    $"encrypt string {"hello".WrapInQuotes()} --long {"wrapped  and . dots .".WrapInQuotes()}",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("--long", "wrapped  and . dots .".WrapInQuotes())]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} --with {"not complete long".WrapInQuotes()} --",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()} --",
                        [("--with", "not complete long".WrapInQuotes())]));
                yield return (
                    $"encrypt string {"hello".WrapInQuotes()} -with {"not complete short".WrapInQuotes()} - -",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()} - -",
                        [("-with", "not complete short".WrapInQuotes())]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} --two-no --value-flags",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("--two-no", string.Empty), ("--value-flags", string.Empty)]));
                yield return (
                    $"encrypt -inside {"args".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-inside", "args".WrapInQuotes()), ("--long-no-value", string.Empty)]));
                yield return (
                    $"encrypt -inside {"any word symb тест 123".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-inside", "any word symb тест 123".WrapInQuotes()), ("--long-no-value", string.Empty)]));
                yield return (
                    $"encrypt -inside {"args  and dots ...".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-inside", "args  and dots ...".WrapInQuotes()), ("--long-no-value", string.Empty)]));
                yield return (
                    $"encrypt --inside-args {"wrapped  and . dots ...".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-no-value",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("--inside-args", "wrapped  and . dots ...".WrapInQuotes()), ("--long-no-value", string.Empty)]));
                yield return (
                    $"encrypt --inside-args {"any word symb тест".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-with-value 256",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("--inside-args", "any word symb тест".WrapInQuotes()), ("--long-with-value", "256")]));
                yield return (
                    $"encrypt --inside-args {"with digits 123".WrapInQuotes()} string {"hello".WrapInQuotes()} --long-with-value 256asd",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("--inside-args", "with digits 123".WrapInQuotes()), ("--long-with-value", "256asd")]));
                yield return (
                    $"encrypt -inside {"with digits 123".WrapInQuotes()} string {"hello".WrapInQuotes()} -shortValue 256",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-inside", "with digits 123".WrapInQuotes()), ("-shortValue", "256")]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -caSEsenSetiVE 123asd",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-caSEsenSetiVE", "123asd")]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -out {"C://path".WrapInQuotes()}",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-out", "C://path".WrapInQuotes())]));
                yield return ($"encrypt string {"hello".WrapInQuotes()} -out {"C:\\path/".WrapInQuotes()}",
                    new SplitExecutionStringInfo($"encrypt string {"hello".WrapInQuotes()}",
                        [("-out", "C:\\path/".WrapInQuotes())]));
            }
        }
    }
}