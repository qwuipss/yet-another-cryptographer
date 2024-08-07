namespace AegisCryptographer.Exceptions;

public class FlagNotSupportedException(string flag) : Exception($"Flag \"{flag}\" is not supported.");