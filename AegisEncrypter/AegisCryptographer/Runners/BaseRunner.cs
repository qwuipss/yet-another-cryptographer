using AegisCryptographer.Commands;
using AegisCryptographer.Commands.Execution;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Commands.Resolvers;
using AegisCryptographer.Configuration;
using AegisCryptographer.Exceptions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;

namespace AegisCryptographer.Runners;

public abstract class BaseRunner(
    IReader reader,
    IWriter writer,
    ICommandExecutor commandExecutor,
    ICommandResolver commandResolver) : IRunner
{
    private readonly IWriter _writer  = writer;
    private readonly ICommandExecutor _commandExecutor = commandExecutor;
    private readonly ICommandResolver _commandResolver  = commandResolver;
    
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
            return;
        }
        catch (IntentionalException exc)
        {
            _writer.WriteException(exc);
            return;
        }

        while (true)
            try
            {
                _commandExecutor.Execute(command);
                break;
            }
            catch (SecretsMismatchException exc)
            {
                _writer.WriteException(exc);
            }
            catch (IntentionalException exc)
            {
                _writer.WriteException(exc);
                break;
            }
    }
}