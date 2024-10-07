using AegisCryptographer.Commands;
using AegisCryptographer.Commands.Execution;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Commands.Resolvers;
using AegisCryptographer.Exceptions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;

namespace AegisCryptographer.Runners;

public abstract class BaseRunner(IReader reader, IWriter writer) : IRunner
{
    protected IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;

    public abstract void Run();

    protected void RunInput(string? input)
    {
        ICommand command;
        var commandResolver = new InputCommandResolver(input, new RegexService(), new CommandFlagsResolver(), Reader, Writer);

        try
        {
            command = commandResolver.Resolve();
        }
        catch (InputEmptyException)
        {
            return;
        }
        catch (IntentionalException exc)
        {
            Writer.WriteException(exc);
            return;
        }

        while (true)
            try
            {
                var executor = new CommandExecutor(Writer);

                executor.Execute(command);

                break;
            }
            catch (SecretsMismatchException exc)
            {
                Writer.WriteException(exc);
            }
            catch (IntentionalException exc)
            {
                Writer.WriteException(exc);
                break;
            }
    }
}