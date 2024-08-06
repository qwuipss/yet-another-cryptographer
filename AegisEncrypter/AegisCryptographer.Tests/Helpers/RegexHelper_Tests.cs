using AegisCryptographer.Helpers;
using FluentAssertions;

namespace AegisCryptographer.Tests.Helpers;

public class RegexHelper_Tests
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.ParameterStringInDoubleQuotes))]
    public void ExtractParameterString_should_extract_parameter_string_in_double_quotes(
        (string Data, string Expected) bundle)
    {
        var str = RegexHelper.ExtractParameterString(bundle.Data);

        str.Should().BeEquivalentTo(bundle.Expected);
    }

    private static class TestCases
    {
        public static IEnumerable<(string Data, string Expected)> ParameterStringInDoubleQuotes
        {
            get
            {
                yield return ($"encrypt string \"\"", "");
                yield return ($"encrypt string \"lorem\"", "lorem");
                yield return ($"encrypt string \"lorem ipsum\"", "lorem ipsum");
                yield return ($"decrypt string \"lorem ipsum \\\"dolor\\\"\"", "lorem ipsum \"dolor\"");
                yield return ($"decrypt string \"lorem ipsum \\\"\\\" \\\"sit\\\"\"", "lorem ipsum \"\" \"sit\"");
            }
        }
    }
}