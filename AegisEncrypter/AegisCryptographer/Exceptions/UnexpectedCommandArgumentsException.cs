namespace AegisCryptographer.Exceptions;

public class UnexpectedCommandArgumentsException(IEnumerable<string> arguments)
    : IntentionalException($"Unexpected argument [{string.Join(", ", arguments)}...].");