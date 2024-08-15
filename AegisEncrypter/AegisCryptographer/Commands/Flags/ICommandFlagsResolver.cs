namespace AegisCryptographer.Commands.Flags;

public interface ICommandFlagsResolver
{
    public ICommandFlag Resolve(string flagKey, string flagValue);
}