using AegisCryptographer.Collections;
using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Extensions;

public static class CommandArgumentsCollectionExtensions
{
    private const int UnexpectedArgumentsExposeMaxCount = 3;

    public static void ThrowIfNotSealed(this ICommandArgumentsCollection commandArgumentsCollection)
    {
        var isSealed = commandArgumentsCollection.IsSealed(out var unexpectedArguments);

        if (isSealed) return;

        if (unexpectedArguments!.Count is 1) throw new UnexpectedCommandArgumentException(unexpectedArguments.Single());

        throw new UnexpectedCommandArgumentsException(unexpectedArguments!.Take(UnexpectedArgumentsExposeMaxCount));
    }
}