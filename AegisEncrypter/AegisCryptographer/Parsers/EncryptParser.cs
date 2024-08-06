using System.Text;
using System.Text.RegularExpressions;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands;
using AegisCryptographer.Commands.Encrypt;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Helpers;
using AegisCryptographer.IO;

namespace AegisCryptographer.Parsers;

public class EncryptParser(CommandArgumentsCollection arguments, IReader reader, IWriter writer)
    : BaseParser(arguments, reader, writer)
{
    private const string CommandName = "encrypt";
    
    private const string ParameterStringArgumentName = "parameter string";
    
    public override ICommand ParseCommand()
    {
        if (Arguments[0] is "string" or "str")
        {
            string? str;
            
            try
            {
                str = Arguments[1..].ExtractParameterString();
            }
            catch (AmbiguousArgumentException)
            {
                throw new CommandInvalidArgumentException(ParameterStringArgumentName, CommandName);
            }

            if (string.IsNullOrEmpty(str))
            {
                throw new CommandInvalidArgumentException(ParameterStringArgumentName, CommandName);
            }
            
            var secret = RequireSecretWithEnsure();

            return new EncryptStringCommand(str, secret);
        }

        throw new CommandInvalidArgumentException(Arguments[0], CommandName);
    }
}