namespace AegisCryptographer.Collections;

public class CommandExecutionStringInfo(
    ICommandArgumentsCollection commandArgumentsCollection,
    ICommandFlagsCollection commandFlagsCollection) : ICommandExecutionStringInfo
{
    public ICommandArgumentsCollection CommandArgumentsCollection { get; } = commandArgumentsCollection;
    public ICommandFlagsCollection CommandFlagsCollection { get; } = commandFlagsCollection;
}