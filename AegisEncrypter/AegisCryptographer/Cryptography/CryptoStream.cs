using AegisCryptographer.Cryptography.Algorithms;

namespace AegisCryptographer.Cryptography;

public class CryptoStream(ICryptoAlgorithm algorithm) : ICryptoStream
{
    public string Encrypt(string str)
    {
        var bytes = Settings.Encoding.GetBytes(str);
        var encrypted = algorithm.Encrypt(bytes);
        var output = Convert.ToBase64String(encrypted);

        return output;
    }

    public string Decrypt(string str)
    {
        var bytes = Convert.FromBase64String(str);
        var decrypted = algorithm.Decrypt(bytes);
        var output = Settings.Encoding.GetString(decrypted);

        return output;
    }
}