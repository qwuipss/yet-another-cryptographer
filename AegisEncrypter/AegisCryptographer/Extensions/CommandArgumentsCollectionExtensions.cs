using AegisCryptographer.Collections;
using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Extensions;

public static class CommandArgumentsCollectionExtensions
{
    private const int UnexpectedArgumentsExposeMaxCount = 3;

    public static void ThrowIfNotSealed(this ICommandArgumentsCollection commandArgumentsCollection)
    {
        if (commandArgumentsCollection.IsSealed(out var unexpectedArguments)) return;

        if (unexpectedArguments!.Count is 1) throw new UnexpectedCommandArgumentException(unexpectedArguments.Single());

        if (unexpectedArguments.Count > UnexpectedArgumentsExposeMaxCount)
            throw new UnexpectedCommandArgumentsException(unexpectedArguments.Take(UnexpectedArgumentsExposeMaxCount));

        throw new UnexpectedCommandArgumentsException(unexpectedArguments, false);
    }
}