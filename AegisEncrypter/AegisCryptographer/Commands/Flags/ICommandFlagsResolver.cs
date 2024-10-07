namespace AegisCryptographer.Commands.Flags;

public interface ICommandFlagsResolver
{
    ICommandFlag Resolve(string flagKey, string flagValue);
}