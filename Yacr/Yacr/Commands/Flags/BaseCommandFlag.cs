using Yacr.Extensions;
using Yacr.Exceptions;
using Yacr.Services;
using RegexService = Yacr.Services.RegexService;

namespace Yacr.Commands.Flags;

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

        private init //todo smth to avoid such logic
        {
            if (!ValueValidationCallback(value)) throw new CommandFlagValueValidationException(Key, value);

            _value = value;
        }
    }

    public override string ToString()
    {
        return $"{this.GetTypeName()}: Key = {Key}, Value = {Value}";
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