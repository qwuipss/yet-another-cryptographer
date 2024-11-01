using Yacr.Commands.Execution;
using Yacr.Cryptography;

namespace Yacr.Commands.Encrypt;

public class EncryptStringCommand(string str, ICryptoStream cryptoStream) : ICommand
{
    private readonly string _string = str;
    private readonly ICryptoStream _cryptoStream = cryptoStream;

    public CommandExecutionResult Execute(Action<string> executionCallback)
    {
        var encryptedString = _cryptoStream.Encrypt(_string);

        executionCallback($"Encrypted string: \"{encryptedString}\".");

        return CommandExecutionResult.Success();
    }
}