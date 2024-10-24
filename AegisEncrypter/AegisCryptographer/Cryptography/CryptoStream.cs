using AegisCryptographer.Configuration;
using AegisCryptographer.Cryptography.Algorithms;
using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Cryptography;

public class CryptoStream(ICryptoAlgorithm algorithm, IConfigurationProvider configurationProvider) : ICryptoStream
{
    private readonly ICryptoAlgorithm _algorithm = algorithm;
    private IConfigurationProvider ConfigurationProvider { get; } = configurationProvider;

    public string Encrypt(string str)
    {
        var bytes = ConfigurationProvider.Encoding.GetBytes(str);
        var encrypted = _algorithm.Encrypt(bytes);
        var output = Convert.ToBase64String(encrypted);

        return output;
    }

    public string Decrypt(string str)
    {
        byte[] bytes;
        
        try
        {
            bytes = Convert.FromBase64String(str);
        }
        catch (FormatException)
        {
            throw new DecryptStringFormatException();
        }
        
        var decrypted = _algorithm.Decrypt(bytes);
        var output = ConfigurationProvider.Encoding.GetString(decrypted);

        return output;
    }
}