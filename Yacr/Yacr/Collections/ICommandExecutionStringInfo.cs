namespace Yacr.Collections;

public interface ICommandExecutionStringInfo
{
    ICommandArgumentsCollection CommandArgumentsCollection { get; }
    ICommandFlagsCollection CommandFlagsCollection { get; }
}