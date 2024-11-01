using Yacr.Collections;
using Yacr.Commands.Flags;
using FluentAssertions;
using NSubstitute;

namespace Yacr.Tests.Collections;

// ReSharper disable once InconsistentNaming
public class CommandFlagsCollection_Tests
{
    // ReSharper disable once InconsistentNaming
    private class TestCommandFlag_ : ICommandFlag
    {
        public string Key => "TestKey";
        public string Value => "TestValue";
    }

    private CommandFlagsCollection _commandFlagsCollection = null!;

    [SetUp]
    public void SetUp()
    {
        _commandFlagsCollection = new CommandFlagsCollection();
    }

    [Test]
    public void AddCommandFlag_should_throw_when_command_flag_already_exists()
    {
        var commandFlag = Substitute.For<ICommandFlag>();

        _commandFlagsCollection.AddCommandFlag(commandFlag);

        Assert.Throws<ArgumentException>(() => _commandFlagsCollection.AddCommandFlag(commandFlag));
    }

    [Test]
    public void GetCommandFlag_should_return_command_flag_as_in_generic_parameter()
    {
        var commandFlag = new TestCommandFlag_();

        _commandFlagsCollection.AddCommandFlag(commandFlag);

        _commandFlagsCollection.GetCommandFlag<TestCommandFlag_>().Should().Be(commandFlag);
    }

    [Test]
    public void GetCommandFlag_should_throw_when_command_flag_does_not_exist()
    {
        Assert.Throws<KeyNotFoundException>(() => _commandFlagsCollection.GetCommandFlag<TestCommandFlag_>());
    }

    [Test]
    public void TryGetCommandFlag_should_return_true_and_type_associated_command_flag_when_command_flag_exists()
    {
        var commandFlag = Substitute.For<ICommandFlag>();

        _commandFlagsCollection.AddCommandFlag(commandFlag);

        _commandFlagsCollection.TryGetCommandFlag(commandFlag, out var typeAssociatedCommandFlag).Should().BeTrue();
        typeAssociatedCommandFlag.Should().Be(commandFlag);
    }

    [Test]
    public void TryGetCommandFlag_should_return_false_and_null_as_type_associated_command_flag_when_command_flag_does_not_exist()
    {
        var commandFlag = Substitute.For<ICommandFlag>();

        _commandFlagsCollection.TryGetCommandFlag(commandFlag, out var typeAssociatedCommandFlag).Should().BeFalse();
        typeAssociatedCommandFlag.Should().BeNull();
    }
    
    [Test]
    public void TryGetCommandFlag_Generic_should_return_true_and_type_associated_command_flag_when_command_flag_exists()
    {
        var commandFlag = new TestCommandFlag_();

        _commandFlagsCollection.AddCommandFlag(commandFlag);

        _commandFlagsCollection.TryGetCommandFlag<TestCommandFlag_>(out var typeAssociatedCommandFlag).Should().BeTrue();
        typeAssociatedCommandFlag.Should().Be(commandFlag);
    }

    [Test]
    public void TryGetCommandFlag_Generic_should_return_false_and_null_as_type_associated_command_flag_when_command_flag_does_not_exist()
    {
        _commandFlagsCollection.TryGetCommandFlag<TestCommandFlag_>(out var typeAssociatedCommandFlag).Should().BeFalse();
        typeAssociatedCommandFlag.Should().BeNull();
    }
}