using Yacr.Commands;
using Yacr.Collections;

namespace Yacr.Cryptography.Algorithms;

public interface ICryptoAlgorithmResolver
{
    ICryptoAlgorithm Resolve(string secret, ICommandFlagsCollection commandFlagsCollection);
}