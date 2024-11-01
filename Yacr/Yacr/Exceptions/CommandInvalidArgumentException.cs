using Yacr.Extensions;

namespace Yacr.Exceptions;

public class CommandInvalidArgumentException(string argument, string command)
    : IntentionalException($"Invalid argument {argument.WrapInQuotes()} for command {command.WrapInQuotes()}.");