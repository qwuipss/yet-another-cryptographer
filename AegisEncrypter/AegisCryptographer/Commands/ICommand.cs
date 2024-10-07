using AegisCryptographer.Commands.Execution;

namespace AegisCryptographer.Commands;

public interface ICommand
{
    CommandExecutionResult Execute(Action<string> executionCallback);
}