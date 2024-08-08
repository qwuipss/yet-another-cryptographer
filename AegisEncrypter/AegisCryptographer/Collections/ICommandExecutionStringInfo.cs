namespace AegisCryptographer.Collections;

public interface ICommandExecutionStringInfo
{
    public ICommandArgumentsCollection CommandArgumentsCollection { get; }
    public ICommandFlagsCollection CommandFlagsCollection { get; }
}