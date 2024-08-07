using AegisCryptographer.Cryptography;

namespace AegisCryptographer.Commands.Decrypt;

public class DecryptStringCommand(string str, ICryptoStream cryptoStream) : ICommand
{
    public void Execute()
    {
        var decrypted = cryptoStream.Decrypt(str);

        Console.WriteLine($"Decrypted: {decrypted}");
    }
}