using AegisCryptographer.Collections;

namespace AegisCryptographer.Commands.Resolvers.Scoped;

public interface IScopedCommandResolver
{
    ICommand Resolve(ICommandExecutionStringInfo commandExecutionStringInfo);
}