namespace AegisCryptographer.Exceptions;

public class SecretTooLongException(int length, int max)
    : Exception($"Secret too long. Current length is {length} bytes but max length is {max} bytes.");