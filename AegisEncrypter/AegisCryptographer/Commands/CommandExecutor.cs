using AegisCryptographer.IO;

namespace AegisCryptographer.Commands;

public class CommandExecutor(IWriter writer) : ICommandExecutor
{
    public void Execute(ICommand command)
    {
        command.Execute(writer.WriteLine);
    }
}