using AegisCryptographer.Collections.Exceptions;
using AegisCryptographer.Commands;
using AegisCryptographer.Exceptions.Parsers;
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
        catch (Exception exc) when (exc is ParserResolveException)
        {
            Writer.WriteException(exc);
            return;
        }

        while (true)
        {
            try
            {
                var command = parser.ParseCommand();
                var executor = new CommandExecutor();
                
                executor.Execute(command);

                break;
            }
            catch (Exception exc) when (exc is SecretsMismatchException)
            {
                Writer.WriteException(exc);
            }
            catch (Exception exc) when (exc is CommandInvalidArgumentException or CommandArgumentMissingException)
            {
                Writer.WriteException(exc);
                break;
            }
        }
    }
}