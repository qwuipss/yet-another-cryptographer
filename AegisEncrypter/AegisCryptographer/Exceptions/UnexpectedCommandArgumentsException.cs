namespace AegisCryptographer.Exceptions;

public class UnexpectedCommandArgumentsException(IEnumerable<string> arguments, bool isArgumentsOmitted = true)
    : IntentionalException(
        $"Unexpected arguments [{string.Join(", ", arguments)}{(isArgumentsOmitted ? ", ..." : string.Empty)}].");