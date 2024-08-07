using System.Security.Cryptography;

namespace AegisCryptographer.Helpers;

public static class CryptoHelper
{
    public static byte[] GetNonce(int size)
    {
        var nonce = new byte[size];

        RandomNumberGenerator.Fill(nonce);

        return nonce;
    }
}