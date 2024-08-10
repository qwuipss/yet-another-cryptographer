using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions.Parsers;

public class CommandInvalidArgumentException(string argument, string command)
    : IntentionalException($"Invalid argument {argument.WrapInQuotes()} for command {command.WrapInQuotes()}.");