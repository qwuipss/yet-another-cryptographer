using AegisCryptographer.Collections;
using AegisCryptographer.Commands;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;

namespace AegisCryptographer.Parsers;

public abstract class BaseParser(CommandArgumentsCollection arguments, IReader reader, IWriter writer) : IParser
{
    protected CommandArgumentsCollection Arguments { get; } = arguments;
    private IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;

    public abstract ICommand ParseCommand();

    protected string RequireSecret()
    {
        return RequireSecret(Writer.WriteEnterSecret);
    }
    
    protected string RequireSecretWithEnsure()
    {
        var secret = RequireSecret();
#if !DEBUG // disable password ensure in debug mode
        var repeatedSecret = RequireSecret(Writer.WriteRepeatSecret);

        if (secret != repeatedSecret)
        {
            throw new SecretsMismatchException();
        }
#endif

        return secret;
    }

    private string RequireSecret(Action writeDelegate)
    {
        string? input;

        do
        {
            writeDelegate();
            input = Reader.ReadSecret();
        } while (StringExtensions.IsNullOrEmptyOrWhitespace(input));

        return input;
    }
}