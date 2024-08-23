using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions;

public class UnexpectedCommandArgumentException(string argument)
    : IntentionalException($"Unexpected argument {argument.WrapInQuotes()}.");