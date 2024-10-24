using AegisCryptographer.Collections;
using AegisCryptographer.Exceptions;
using FluentAssertions;

namespace AegisCryptographer.Tests.Collections;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class CommandArgumentsCollection_Tests
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Initialization))]
    public void CommandArgumentsCollection_count_should_be_same_as_initialization_array_length(
        string[] commandArguments)
    {
        new CommandArgumentsCollection(commandArguments).Count.Should().Be(commandArguments.Length);
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.NextMoveAndReturn))]
    public void Next_should_return_elements_and_move_pointer(string[] commandArguments)
    {
        var commandArgumentsCollection = new CommandArgumentsCollection(commandArguments);

        for (var i = 0; i < commandArgumentsCollection.Count; i++)
            commandArguments[i].Should().BeEquivalentTo(commandArgumentsCollection.Next(string.Empty));
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.ElementsCountEnds))]
    public void Next_should_throw_when_elements_count_ends(string[] commandArguments)
    {
        var commandArgumentsCollection = new CommandArgumentsCollection(commandArguments);

        for (var i = 0; i < commandArgumentsCollection.Count; i++) commandArgumentsCollection.Next(string.Empty);

        Assert.Throws<CommandArgumentMissingException>(() => commandArgumentsCollection.Next(string.Empty));
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.IsSealed))]
    public void IsSealed_should_correctly_return_seal_result_and_define_unexpected_arguments(
        (string[] CommandArguments, int NextRequireCount, bool IsSealed, List<string>? ExpectedUnexpectedArguments)
            bundle)
    {
        var commandArgumentsCollection = new CommandArgumentsCollection(bundle.CommandArguments);

        for (var i = 0; i < bundle.NextRequireCount; i++) commandArgumentsCollection.Next(string.Empty);

        commandArgumentsCollection.IsSealed(out var unexpectedArguments).Should().Be(bundle.IsSealed);
        unexpectedArguments.Should().BeEquivalentTo(bundle.ExpectedUnexpectedArguments);
    }

    private static class TestCases
    {
        public static IEnumerable<string[]> Initialization
        {
            get
            {
                yield return [];
                yield return ["encrypt", "str"];
                yield return ["encrypt", "str", "hello world"];
                yield return ["encrypt", "str", "hello world", "stub1", "stub2"];
            }
        }

        public static IEnumerable<string[]> NextMoveAndReturn => Initialization;
        public static IEnumerable<string[]> ElementsCountEnds => Initialization;

        public static IEnumerable<(string[] CommandArguments, int NextRequireCount, bool IsSealed, List<string>?
            ExpectedUnexpectedArguments)> IsSealed
        {
            get
            {
                yield return ([], 0, true, null);
                yield return (["enc"], 1, true, null);
                yield return (["enc", "str", "hello world"], 3, true, null);
                yield return (["enc", "str", "hello world", "stub1"], 4, true, null);
                yield return (["encrypt"], 0, false, ["encrypt"]);
                yield return (["enc", "str", "hello world", "stub1", "stub2"], 1, false,
                    ["str", "hello world", "stub1", "stub2"]);
                yield return (["enc", "str", "hello world", "stub1", "stub2"], 3, false, ["stub1", "stub2"]);
                yield return (["enc", "str", "hello world", "stub1", "stub2"], 4, false, ["stub2"]);
            }
        }
    }
}