namespace AegisCryptographer.Commands;

public class CommandExecutor : ICommandExecutor
{
    public void Execute(ICommand command)
    {
        command.Execute();
    }
}