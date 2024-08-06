namespace AegisCryptographer.Commands;

public interface ICommandExecutor
{
    public void Execute(ICommand command);
}