using AegisCryptographer.IO;

namespace AegisCryptographer.Commands.Execution;

public class CommandExecutor(IWriter writer) : ICommandExecutor
{
    public void Execute(ICommand command)
    {
        command.Execute(writer.WriteLine);
    }
}