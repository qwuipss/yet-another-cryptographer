using AegisCryptographer.Commands.Flags;

namespace AegisCryptographer.Collections;

public interface ICommandFlagsCollection
{
    void AddCommandFlag(ICommandFlag commandFlag);
    ICommandFlag GetCommandFlag<TCommandFlag>() where TCommandFlag : ICommandFlag;
    bool TryGetCommandFlag<TCommandFlag>(out ICommandFlag? typeAssociatedCommandFlag) where TCommandFlag : ICommandFlag;
    bool TryGetCommandFlag(ICommandFlag commandFlag, out ICommandFlag? typeAssociatedCommandFlag);
}