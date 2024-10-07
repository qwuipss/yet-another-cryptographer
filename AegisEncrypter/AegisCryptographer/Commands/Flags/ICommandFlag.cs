namespace AegisCryptographer.Commands.Flags;

public interface ICommandFlag
{
    string Key { get; }
    string Value { get; }
}