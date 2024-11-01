using Yacr.Extensions;

namespace Yacr.Exceptions;

public class AlgorithmNotSupportedException(string algorithm)
    : IntentionalException($"Algorithm {algorithm.WrapInQuotes()} is not supported.");