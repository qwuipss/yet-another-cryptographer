using AegisCryptographer.Commands.Execution;

namespace AegisCryptographer.Commands;

public interface ICommand
{
    public CommandExecutionResult Execute(Action<string> executionCallback);
}