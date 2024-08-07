namespace AegisCryptographer.Exceptions;

public class FlagDuplicateException(string duplicateFlag, string flag)
    : Exception($"Flag \"{duplicateFlag}\" already defined by flag \"{flag}\".");