using Yacr.Exceptions;
using static Yacr.Commands.Flags.FlagsKeys;

namespace Yacr.Commands.Flags;

public class CommandFlagsResolver : ICommandFlagsResolver
{
    public ICommandFlag Resolve(string flagKey, string flagValue)
    {
        return flagKey switch
        {
            AlgorithmShortKey or AlgorithmLongKey => new AlgorithmCommandFlag(flagKey, flagValue),
            _ => throw new FlagNotSupportedException(flagKey)
        };
    }
}