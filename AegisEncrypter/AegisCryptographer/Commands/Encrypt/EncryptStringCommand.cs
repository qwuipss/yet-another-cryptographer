using AegisCryptographer.Commands.Decrypt;
using AegisCryptographer.Cryptography;

namespace AegisCryptographer.Commands.Encrypt;

public class EncryptStringCommand(string str, ICryptoStream cryptoStream) : ICommand
{
    public void Execute()
    {
        var encrypted = cryptoStream.Encrypt(str);

        Console.WriteLine($"Encrypted: {encrypted}");

        new DecryptStringCommand(encrypted, cryptoStream).Execute();
    }
}