using Yacr.Extensions;
using FluentAssertions;

namespace Yacr.Tests.Extensions;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class EnumerableExtensions_Tests
{
    [TestCase(new[] { -1, -10, -17 })]
    [TestCase(new[] { 100, 1, 1, 13 })]
    [TestCase(new[] { 1, 3, 7, 11, 29 })]
    public void ForEach_should_enumerate_whole_enumerable(int[] enumerable)
    {
        var sum = 0;
        var iterations = 0;

        enumerable.ForEach(x =>
        {
            x.Should().Be(enumerable[iterations++]);
            sum += x;
        });

        sum.Should().Be(enumerable.Sum());
        iterations.Should().Be(enumerable.Length);
    }
}