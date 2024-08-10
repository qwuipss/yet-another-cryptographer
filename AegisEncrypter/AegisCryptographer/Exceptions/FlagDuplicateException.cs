using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions;

public class FlagDuplicateException(string duplicateFlag, string existedFlag)
    : IntentionalException(
        $"Flag {duplicateFlag.WrapInQuotes()} already defined by flag {existedFlag.WrapInQuotes()}.");