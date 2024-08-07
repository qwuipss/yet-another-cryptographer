namespace AegisCryptographer.Exceptions;

public class CommandFlagValueValidationException(string flag, string value)
    : Exception($"Value \"{value}\" is not valid for flag \"{flag}\".");