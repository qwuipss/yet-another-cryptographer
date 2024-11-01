using Yacr.Extensions;
using Yacr.Commands;
using Yacr.Commands.Execution;
using Yacr.Commands.Resolvers;
using Yacr.Debug;
using Yacr.Exceptions;
using Yacr.IO;

namespace Yacr.Runners;

public abstract class BaseRunner(
    IReader reader,
    IWriter writer,
    IDebugLogFactory debugLogFactory,
    ICommandExecutor commandExecutor,
    ICommandResolver commandResolver) : IRunner
{
    private readonly IWriter _writer = writer;
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly ICommandResolver _commandResolver = commandResolver;
    private readonly IDebugLog _debugLog = debugLogFactory.ForContext<BaseRunner>();

    protected IReader Reader { get; } = reader;

    public abstract void Run();

    protected void RunInput(string? input)
    {
        ICommand command;

        try
        {
            command = _commandResolver.Resolve(input);
        }
        catch (InputEmptyException)
        {
            DebugLogEmptyInputReceived();
            return;
        }
        catch (IntentionalException exception)
        {
            DebugLogIntentionalExceptionHandled(exception);
            _writer.WriteIntentionalException(exception);
            return;
        }

        while (true)
            try
            {
                _commandExecutor.Execute(command);
                break;
            }
            catch (SecretsMismatchException exception)
            {
                DebugLogSecretsMismatch();
                _writer.WriteIntentionalException(exception);
            }
            catch (IntentionalException exception)
            {
                DebugLogIntentionalExceptionHandled(exception);
                _writer.WriteIntentionalException(exception);
                break;
            }
    }

    #region DebugLog

    private void DebugLogEmptyInputReceived()
    {
        _debugLog.Info("Empty input received.");
    }

    private void DebugLogIntentionalExceptionHandled(IntentionalException exception)
    {
        _debugLog.Info($"Intentional exception handled: {exception.GetTypeName()}.");
    }

    private void DebugLogSecretsMismatch()
    {
        _debugLog.Info("Secrets mismatch.");
    }

    #endregion
}