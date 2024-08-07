namespace AegisCryptographer.Commands.Flags;

public interface ICommandFlag
{
    public string Key { get; }
    public string Value { get; }
}