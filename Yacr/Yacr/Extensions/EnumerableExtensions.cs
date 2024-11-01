namespace Yacr.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> callback) //todo maybe remove
    {
        foreach (var elem in enumerable) callback(elem);
    }
}