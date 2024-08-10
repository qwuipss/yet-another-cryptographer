using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions.Parsers;

public class ParserResolveException(string command)
    : IntentionalException($"Unable to resolve command: {command.WrapInQuotes()}.");