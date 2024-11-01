using Yacr.Extensions;

namespace Yacr.Exceptions;

public class FlagDuplicateException(string duplicateFlag, string existedFlag)
    : IntentionalException(
        $"Flag {duplicateFlag.WrapInQuotes()} already defined by flag {existedFlag.WrapInQuotes()}.");