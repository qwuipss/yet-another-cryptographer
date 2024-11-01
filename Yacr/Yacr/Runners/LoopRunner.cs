using Yacr.Extensions;
using Yacr.Commands.Execution;
using Yacr.Commands.Resolvers;
using Yacr.Debug;
using Yacr.IO;
using static Yacr.Commands.CommandsTokens;

namespace Yacr.Runners;

public class LoopRunner(
    IReader reader,
    IWriter writer,
    IDebugLogFactory debugLogFactory,
    ICommandExecutor commandExecutor,
    ICommandResolver commandResolver) : BaseRunner(reader, writer, debugLogFactory, commandExecutor, commandResolver)
{
    private readonly IDebugLog _debugLog = debugLogFactory.ForContext<LoopRunner>();

    public override void Run()
    {
        while (true)
        {
            var input = Reader.ReadLine();
            DebugLogReceivedInput(input);

            if (input is ExitShortToken or ExitLongToken) break; //todo execute as command

            RunInput(input);
        }
    }

    #region DebugLog

    private void DebugLogReceivedInput(string? input)
    {
        if (input.IsNullOrEmptyOrWhitespace())
        {
            _debugLog.Info("Empty input received.");
            return;
        }

        _debugLog.Info($"Received input: {input}.");
    }

    #endregion
}