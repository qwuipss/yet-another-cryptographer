using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Commands.Resolvers;
using AegisCryptographer.IO;
using NSubstitute;

namespace AegisCryptographer.Tests.Commands.Resolvers;

[TestFixture]
public class CommandResolver_Tests
{
    public void A()
    {
        var flagsResolver = Substitute.For<ICommandFlagsResolver>();
        var reader = Substitute.For<IReader>();
        var writer = Substitute.For<IWriter>();
        var commandResolver = new CommandResolver("encrypt str asd", flagsResolver, reader, writer);

        commandResolver.Resolve();
    }
}