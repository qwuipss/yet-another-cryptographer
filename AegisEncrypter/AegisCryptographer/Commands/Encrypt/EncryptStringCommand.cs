using AegisCryptographer.Commands.Execution;
using AegisCryptographer.Cryptography;

namespace AegisCryptographer.Commands.Encrypt;

public class EncryptStringCommand(string str, ICryptoStream cryptoStream) : ICommand
{
    private string Str { get; } = str;
    private ICryptoStream CryptoStream { get; } = cryptoStream;

    public CommandExecutionResult Execute(Action<string> executionCallback)
    {
        var encrypted = CryptoStream.Encrypt(Str);

        executionCallback($"Encrypted string: \"{encrypted}\".");

        return CommandExecutionResult.Success();
    }
}