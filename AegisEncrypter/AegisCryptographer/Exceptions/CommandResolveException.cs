using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions;

public class CommandResolveException(string command)
    : IntentionalException($"Unable to resolve command {command.WrapInQuotes()}.");