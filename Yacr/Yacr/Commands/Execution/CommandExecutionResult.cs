namespace Yacr.Commands.Execution;

public class CommandExecutionResult(CommandExecutionStatus status)
{
    public CommandExecutionStatus Status { get; } = status;

    public static CommandExecutionResult Success()
    {
        return new CommandExecutionResult(CommandExecutionStatus.Success);
    }

    public static CommandExecutionResult Error()
    {
        return new CommandExecutionResult(CommandExecutionStatus.Error);
    }
}