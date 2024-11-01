using Yacr.Extensions;

namespace Yacr.Exceptions;

public class CommandFlagValueValidationException(string flag, string value)
    : IntentionalException($"Value {value.WrapInQuotes()} is not valid for flag {flag.WrapInQuotes()}.");