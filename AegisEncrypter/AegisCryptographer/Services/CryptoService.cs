using System.Security.Cryptography;

namespace AegisCryptographer.Services;

public class CryptoService : ICryptoService
{
    public byte[] GetRandomNonce(int size)
    {
        var nonce = new byte[size];

        RandomNumberGenerator.Fill(nonce);

        return nonce;
    }
}