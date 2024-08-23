using AegisCryptographer.Commands.Execution;
using AegisCryptographer.Cryptography;

namespace AegisCryptographer.Commands.Decrypt;

public class DecryptStringCommand(string str, ICryptoStream cryptoStream) : ICommand
{
    public CommandExecutionResult Execute(Action<string> executionCallback)
    {
        var decrypted = cryptoStream.Decrypt(str);

        executionCallback($"Decrypted string: \"{decrypted}\".");

        return CommandExecutionResult.Success();
    }
}