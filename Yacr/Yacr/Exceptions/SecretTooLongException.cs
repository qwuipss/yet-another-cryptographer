namespace Yacr.Exceptions;

public class SecretTooLongException(int length, int max)
    : IntentionalException($"Secret too long. Current length is {length} bytes but max length is {max} bytes.");