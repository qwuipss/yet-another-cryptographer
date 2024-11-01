using Yacr.Commands.Flags;

namespace Yacr.Collections;

public class CommandFlagsCollection : ICommandFlagsCollection
{
    private readonly Dictionary<Type, ICommandFlag> _commandFlags = new();

    public void AddCommandFlag(ICommandFlag commandFlag)
    {
        _commandFlags.Add(commandFlag.GetType(), commandFlag);
    }

    public ICommandFlag GetCommandFlag<TCommandFlag>() where TCommandFlag : ICommandFlag
    {
        return _commandFlags[typeof(TCommandFlag)];
    }

    public bool TryGetCommandFlag<TCommandFlag>(out ICommandFlag? typeAssociatedCommandFlag) where TCommandFlag : ICommandFlag
    {
        return _commandFlags.TryGetValue(typeof(TCommandFlag), out typeAssociatedCommandFlag);
    }

    public bool TryGetCommandFlag(ICommandFlag commandFlag, out ICommandFlag? typeAssociatedCommandFlag)
    {
        return _commandFlags.TryGetValue(commandFlag.GetType(), out typeAssociatedCommandFlag);
    }
}