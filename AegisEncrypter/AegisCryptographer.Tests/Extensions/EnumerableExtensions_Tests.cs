using AegisCryptographer.Extensions;
using FluentAssertions;

namespace AegisCryptographer.Tests.Extensions;

public class EnumerableExtensions_Tests
{
    [TestCase(new[] {-1, -10, -17})]
    [TestCase(new[] {100, 1, 1, 13})]
    [TestCase(new[] {1, 3, 7, 11, 29})]
    public void ForEach_should_enumerate_whole_enumerable(int[] enumerable)
    {
        var sum = 0;
        var iterations = 0;
        
        EnumerableExtensions.ForEach(enumerable, x =>
        {
            sum += x;
            iterations++;
        });

        sum.Should().Be(enumerable.Sum());
        iterations.Should().Be(enumerable.Length);
    }
}