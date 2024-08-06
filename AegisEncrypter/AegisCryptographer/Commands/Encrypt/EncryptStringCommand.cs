using System.Security.Cryptography;
using System.Text;
using AegisCryptographer.Extensions;

namespace AegisCryptographer.Commands.Encrypt;

public class EncryptStringCommand(string str, string secret) : ICommand
{
    public void Execute()
    {
        var tagBytesSize = AesGcm.TagByteSizes.MaxSize;
        var aes = new AesGcm(secret.ToCipherKeyWithAutoPadding(), tagBytesSize);
        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(nonce);
        var cipherText = new byte[Config.Encoding.GetByteCount(str)];
        var tag = new byte[tagBytesSize];

        aes.Encrypt(nonce, str.ToByteReadOnlySpan(), cipherText, tag);

        var plainText = new byte[cipherText.Length];
        
        aes.Decrypt(nonce, cipherText, tag, plainText);
        
        var encrypted = Convert.ToBase64String(cipherText);
        var decrypted = Config.Encoding.GetString(plainText);
        
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {decrypted}");
    }
}