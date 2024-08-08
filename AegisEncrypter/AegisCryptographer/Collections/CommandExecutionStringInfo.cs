namespace AegisCryptographer.Collections;

public class CommandExecutionStringInfo(ICommandArgumentsCollection argumentsCollection, ICommandFlagsCollection flagsCollection) : ICommandExecutionStringInfo
{
    public ICommandArgumentsCollection CommandArgumentsCollection { get; } = argumentsCollection;
    public ICommandFlagsCollection CommandFlagsCollection { get; } = flagsCollection;
}