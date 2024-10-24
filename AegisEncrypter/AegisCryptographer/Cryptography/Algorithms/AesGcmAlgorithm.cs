using System.Security.Cryptography;
using AegisCryptographer.Configuration;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using AegisCryptographer.Services;

namespace AegisCryptographer.Cryptography.Algorithms;

public class AesGcmAlgorithm(string secret, ICryptoService cryptoService, IConfigurationProvider configurationProvider) : ICryptoAlgorithm
{
    private static readonly int TagBytesSize = AesGcm.TagByteSizes.MaxSize; //todo reduce for files 
    private static readonly int NonceSize = AesGcm.NonceByteSizes.MaxSize;

    private readonly AesGcm _aes = new(secret.ToPaddedSecretKey(configurationProvider.Encoding), TagBytesSize);
    private readonly ICryptoService _cryptoService = cryptoService;

    public byte[] Encrypt(byte[] data)
    {
        var nonce = _cryptoService.GetRandomNonce(NonceSize);
        var cipher = new byte[data.Length];
        var tag = new byte[TagBytesSize];

        _aes.Encrypt(nonce, data, cipher, tag);

        var output = new byte[nonce.Length + cipher.Length + tag.Length];

        Buffer.BlockCopy(nonce, 0, output, 0, nonce.Length);
        Buffer.BlockCopy(cipher, 0, output, nonce.Length, cipher.Length);
        Buffer.BlockCopy(tag, 0, output, nonce.Length + cipher.Length, tag.Length);

        return output;
    }

    public byte[] Decrypt(byte[] data)
    {
        var nonce = new ReadOnlySpan<byte>(data, 0, NonceSize);
        var cipher = new ReadOnlySpan<byte>(data, NonceSize, data.Length - NonceSize - TagBytesSize);
        var tag = new ReadOnlySpan<byte>(data, NonceSize + cipher.Length, TagBytesSize);
        var output = new byte[cipher.Length];

        try
        {
            _aes.Decrypt(nonce, cipher, tag, output);
        }
        catch (AuthenticationTagMismatchException)
        {
            throw new CryptoAlgorithmException("Decryption failed. Authentication tag mismatch.");
        }

        return output;
    }
}