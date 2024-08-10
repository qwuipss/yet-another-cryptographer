using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions;

public class FlagNotSupportedException(string flag)
    : IntentionalException($"Flag {flag.WrapInQuotes()} is not supported.");