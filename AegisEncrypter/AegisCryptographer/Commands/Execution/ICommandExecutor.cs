namespace AegisCryptographer.Commands.Execution;

public interface ICommandExecutor
{
    public void Execute(ICommand command);
}