using AegisCryptographer.Cryptography.Algorithms;

namespace AegisCryptographer.Exceptions;

public class AlgorithmNotSupportedException(CryptoAlgorithm algorithm)
    : Exception($"Algorithm \"{algorithm}\" is not supported.");