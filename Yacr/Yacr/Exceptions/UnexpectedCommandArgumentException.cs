using Yacr.Extensions;

namespace Yacr.Exceptions;

public class UnexpectedCommandArgumentException(string argument)
    : IntentionalException($"Unexpected argument {argument.WrapInQuotes()}.");