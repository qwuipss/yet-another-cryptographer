using AegisCryptographer.Collections;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;

namespace AegisCryptographer.Tests.Extensions;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class CommandArgumentsCollectionExtensions_Tests
{
    [TestCase(new[] { "encrypt", "str" }, 1)]
    [TestCase(new[] { "encrypt", "str", "hello world" }, 2)]
    [TestCase(new[] { "encrypt", "str", "hello world", "" }, 3)]
    public void ThrowIfNotSealed_should_throw_when_unexpected_argument_provided(string[] commandArguments, int nextRequireCount)
    {
        Assert.Throws<UnexpectedCommandArgumentException>(() =>
            CreateCommandArgumentsCollectionAndRunNext(commandArguments, nextRequireCount).ThrowIfNotSealed());
    }


    [TestCase(new[] { "encrypt", "str" }, 0)]
    [TestCase(new[] { "encrypt", "str", "hello world" }, 1)]
    [TestCase(new[] { "encrypt", "str", "hello world", "", "" }, 3)]
    public void ThrowIfNotSealed_should_throw_when_unexpected_arguments_provided(string[] commandArguments, int nextRequireCount)
    {
        Assert.Throws<UnexpectedCommandArgumentsException>(() =>
            CreateCommandArgumentsCollectionAndRunNext(commandArguments, nextRequireCount).ThrowIfNotSealed());
    }


    [TestCase(new[] { "encrypt", "str", "hello world" }, 3)]
    [TestCase(new[] { "encrypt", "str", "hello world", "" }, 4)]
    [TestCase(new[] { "encrypt", "str", "hello world", "", "" }, 5)]
    public void ThrowIfNotSealed_should_not_throw_when_unexpected_arguments_not_provided(string[] arguments, int nextRequireCount)
    {
        Assert.DoesNotThrow(() => CreateCommandArgumentsCollectionAndRunNext(arguments, nextRequireCount).ThrowIfNotSealed());
    }


    private static CommandArgumentsCollection CreateCommandArgumentsCollectionAndRunNext(IList<string> commandArguments, int nextRequireCount)
    {
        var commandArgumentsCollection = new CommandArgumentsCollection(commandArguments);

        for (var i = 0; i < nextRequireCount; i++) commandArgumentsCollection.Next(string.Empty);

        return commandArgumentsCollection;
    }
}