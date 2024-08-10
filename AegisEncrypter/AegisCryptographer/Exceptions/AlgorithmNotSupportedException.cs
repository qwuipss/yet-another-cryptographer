using AegisCryptographer.Extensions;

namespace AegisCryptographer.Exceptions;

public class AlgorithmNotSupportedException(string algorithm)
    : IntentionalException($"Algorithm {algorithm.WrapInQuotes()} is not supported.");