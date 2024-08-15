using AegisCryptographer.Exceptions;
using static AegisCryptographer.Commands.Flags.FlagsKeys;

namespace AegisCryptographer.Commands.Flags;

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