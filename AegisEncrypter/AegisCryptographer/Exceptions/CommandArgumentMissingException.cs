using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions;

public class CommandArgumentMissingException(string commandToken)
    : IntentionalException($"Command argument {commandToken.WrapInQuotes()} missing.");