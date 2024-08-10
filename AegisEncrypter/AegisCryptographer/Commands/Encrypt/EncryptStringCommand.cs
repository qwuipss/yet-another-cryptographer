using AegisCryptographer.Cryptography;

namespace AegisCryptographer.Commands.Encrypt;

public class EncryptStringCommand(string str, ICryptoStream cryptoStream) : ICommand
{
    public CommandExecutionResult Execute(Action<string> executionCallback)
    {
        var encrypted = cryptoStream.Encrypt(str);

        executionCallback($"Encrypted string: \"{encrypted}\"");

        return CommandExecutionResult.Success();
    }
}