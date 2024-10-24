using AegisCryptographer.Collections;
using AegisCryptographer.Commands;

namespace AegisCryptographer.Cryptography.Algorithms;

public interface ICryptoAlgorithmResolver
{
    ICryptoAlgorithm Resolve(string secret, ICommandFlagsCollection commandFlagsCollection);
}