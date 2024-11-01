using Yacr.Extensions;

namespace Yacr.Exceptions;

public class FlagNotSupportedException(string flag)
    : IntentionalException($"Flag {flag.WrapInQuotes()} is not supported.");