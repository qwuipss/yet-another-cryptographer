using AegisCryptographer.Exceptions;
using RegexService = AegisCryptographer.Services.RegexService;

namespace AegisCryptographer.Commands.Flags;

public abstract class BaseCommandFlag : ICommandFlag
{
    private readonly string _value = null!;

    protected BaseCommandFlag(string key, string value)
    {
        Key = key;
        Value = value;
    }

    protected virtual Func<string, bool> ValueValidationCallback => value => RegexService.GetCommandFlagDefaultValueValidationRegex().IsMatch(value);

    public string Key { get; }

    public string Value
    {
        get => _value;

        private init
        {
            if (!ValueValidationCallback(value)) throw new CommandFlagValueValidationException(Key, value);

            _value = value;
        }
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj?.GetType() == GetType();
    }
}