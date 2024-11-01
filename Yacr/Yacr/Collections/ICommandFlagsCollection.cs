using Yacr.Commands.Flags;

namespace Yacr.Collections;

public interface ICommandFlagsCollection
{
    void AddCommandFlag(ICommandFlag commandFlag);
    ICommandFlag GetCommandFlag<TCommandFlag>() where TCommandFlag : ICommandFlag;
    bool TryGetCommandFlag<TCommandFlag>(out ICommandFlag? typeAssociatedCommandFlag) where TCommandFlag : ICommandFlag;
    bool TryGetCommandFlag(ICommandFlag commandFlag, out ICommandFlag? typeAssociatedCommandFlag);
}