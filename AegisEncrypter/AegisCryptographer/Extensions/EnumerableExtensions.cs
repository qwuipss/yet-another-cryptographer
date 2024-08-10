namespace AegisCryptographer.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> callback)
    {
        foreach (var elem in enumerable) callback(elem);
    }
}