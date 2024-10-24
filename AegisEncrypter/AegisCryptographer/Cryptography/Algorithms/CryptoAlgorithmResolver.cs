using AegisCryptographer.Collections;
using AegisCryptographer.Configuration;
using AegisCryptographer.Services;

namespace AegisCryptographer.Cryptography.Algorithms;

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