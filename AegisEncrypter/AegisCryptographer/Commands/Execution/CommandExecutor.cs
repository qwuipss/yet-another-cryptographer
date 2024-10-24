using AegisCryptographer.IO;

namespace AegisCryptographer.Commands.Execution;

public class CommandExecutor(IWriter writer) : ICommandExecutor
{
    private readonly IWriter _writer = writer;

    public void Execute(ICommand command)
    {
        command.Execute(_writer.WriteLine);
    }
}