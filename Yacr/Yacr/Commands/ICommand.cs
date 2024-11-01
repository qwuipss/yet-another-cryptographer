using Yacr.Commands.Execution;

namespace Yacr.Commands;

public interface ICommand
{
    CommandExecutionResult Execute(Action<string> executionCallback);
}