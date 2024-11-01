using Yacr.Collections;
using Yacr.Configuration;
using Yacr.Services;

namespace Yacr.Cryptography.Algorithms;

public class CryptoAlgorithmResolver(ICryptoService cryptoService, IConfigurationProvider configurationProvider) : ICryptoAlgorithmResolver
{
    private readonly ICryptoService _cryptoService = cryptoService;
    private readonly IConfigurationProvider _configurationProvider = configurationProvider;

    public ICryptoAlgorithm Resolve(string secret, ICommandFlagsCollection commandFlagsCollection)
    {
        // commandFlagsCollection.
        
        return new AesGcmAlgorithm(secret, _cryptoService, _configurationProvider);
    }
}