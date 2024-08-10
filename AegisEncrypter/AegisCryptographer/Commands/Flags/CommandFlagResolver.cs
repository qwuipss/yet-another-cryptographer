using AegisCryptographer.Exceptions;
using static AegisCryptographer.Commands.Flags.FlagsKeys;

namespace AegisCryptographer.Commands.Flags;

public static class CommandFlagResolver
{
    public static ICommandFlag ResolveCommandFlag((string Flag, string Value) flagBundle)
    {
        return ResolveCommandFlag(flagBundle.Flag, flagBundle.Value);
    }

    private static ICommandFlag ResolveCommandFlag(string flagKey, string flagValue)
    {
        return flagKey switch
        {
            AlgorithmShortKey or AlgorithmLongKey => new AlgorithmCommandFlag(flagKey, flagValue),
            _ => throw new FlagNotSupportedException(flagKey)
        };
    }
}