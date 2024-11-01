namespace Yacr.Commands.Execution;

public interface ICommandExecutor
{
    void Execute(ICommand command);
}