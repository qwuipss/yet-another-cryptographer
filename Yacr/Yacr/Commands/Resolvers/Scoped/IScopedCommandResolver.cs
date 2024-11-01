using Yacr.Collections;

namespace Yacr.Commands.Resolvers.Scoped;

public interface IScopedCommandResolver
{
    ICommand Resolve(ICommandExecutionStringInfo commandExecutionStringInfo);
}