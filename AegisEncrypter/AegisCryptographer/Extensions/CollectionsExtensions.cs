using AegisCryptographer.Collections;

namespace AegisCryptographer.Extensions;

public static class CollectionsExtensions
{
    public static CommandArgumentsCollection ToCommandArgumentsCollection(this IList<string> collection)
    {
        return new CommandArgumentsCollection(collection);
    }
}