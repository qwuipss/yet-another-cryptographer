namespace AegisCryptographer.Commands.Execution;

public interface ICommandExecutor
{
    void Execute(ICommand command);
}