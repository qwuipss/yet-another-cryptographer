using Yacr.Extensions;

namespace Yacr.Exceptions;

public class CommandArgumentMissingException(string commandToken)
    : IntentionalException($"Command argument {commandToken.WrapInQuotes()} missing.");