using AegisCryptographer.Commands;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Helpers;
using AegisCryptographer.IO;
using AegisCryptographer.Parsers;

namespace AegisCryptographer.Runners;

public abstract class BaseRunner(IReader reader, IWriter writer) : IRunner
{
    protected IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;

    public abstract void Run();

    protected void RunInput(string? input)
    {
        IParser parser;

        try
        {
            parser = ParsersHelper.CreateParser(input, Reader, Writer);
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
                var command = parser.ParseCommand();
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