using AegisCryptographer.Commands.Execution;
using AegisCryptographer.Cryptography;

namespace AegisCryptographer.Commands.Decrypt;

public class DecryptStringCommand(string str, ICryptoStream cryptoStream) : ICommand
{
    private readonly string _string = str;
    private readonly ICryptoStream _cryptoStream = cryptoStream;
    
    public CommandExecutionResult Execute(Action<string> executionCallback)
    {
        var decryptedString = _cryptoStream.Decrypt(_string);

        executionCallback($"Decrypted string: \"{decryptedString}\".");

        return CommandExecutionResult.Success();
    }
}