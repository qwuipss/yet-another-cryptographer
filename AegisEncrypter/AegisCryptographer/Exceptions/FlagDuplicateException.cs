namespace AegisCryptographer.Exceptions;

public class FlagDuplicateException(string duplicateFlag, string existedFlag)
    : Exception($"Flag \"{duplicateFlag}\" already defined by flag \"{existedFlag}\".");