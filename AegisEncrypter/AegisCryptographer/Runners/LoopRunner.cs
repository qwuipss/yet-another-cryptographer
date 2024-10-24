using AegisCryptographer.Commands.Execution;
using AegisCryptographer.Commands.Resolvers;
using AegisCryptographer.IO;
using static AegisCryptographer.Commands.CommandsTokens;

namespace AegisCryptographer.Runners;

public class LoopRunner(
    IReader reader,
    IWriter writer,
    ICommandExecutor commandExecutor,
    ICommandResolver commandResolver) : BaseRunner(reader, writer, commandExecutor, commandResolver)
{
    public override void Run()
    {
        while (true)
        {
            var input = Reader.ReadLine();

            if (input is ExitShortToken or ExitLongToken) break; //todo execute as command

            RunInput(input);
        }
    }
}