using AegisCryptographer.Collections;
using AegisCryptographer.Commands;
using AegisCryptographer.Commands.Encrypt;
using AegisCryptographer.Cryptography;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Helpers;
using AegisCryptographer.IO;

namespace AegisCryptographer.Parsers;

public class EncryptParser(CommandArgumentsCollection arguments, IReader reader, IWriter writer)
    : BaseParser(arguments, reader, writer)
{
    private const string CommandName = "encrypt";

    private const string ArgumentStringName = "argument string";

    public override ICommand ParseCommand()
    {
        if (Arguments[0] is "string" or "str")
        {
            string? str;

            try
            {
                str = Arguments[1..].ExtractArgumentString();
            }
            catch (AmbiguousArgumentException)
            {
                throw new CommandInvalidArgumentException(ArgumentStringName, CommandName);
            }

            if (string.IsNullOrEmpty(str)) throw new CommandInvalidArgumentException(ArgumentStringName, CommandName);

            var secret = RequireSecretWithEnsure();
            var algorithm = ResolveCryptoAlgorithm(secret);

            return new EncryptStringCommand(str, new CryptoStream(algorithm));
        }

        throw new CommandInvalidArgumentException(Arguments[0], CommandName);
    }
}