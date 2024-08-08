using System.Text.RegularExpressions;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Helpers;

namespace AegisCryptographer.Commands.Flags;

public abstract class BaseCommandFlag : ICommandFlag
{
    private readonly string _value = null!;

    protected BaseCommandFlag(string key, string value)
    {
        Key = key;
        Value = value;
    }

    private static Regex ValueValidationRegex => RegexHelper.GetCommandFlagDefaultValueValidationRegex();

    public string Key { get; }

    public string Value
    {
        get => _value;

        private init
        {
            if (!ValueValidationRegex.IsMatch(value)) throw new CommandFlagValueValidationException(Key, value);

            _value = value;
        }
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ICommandFlag && obj.GetHashCode() == GetHashCode();
    }

    public static ICommandFlag ResolveCommandFlag(string flagKey, string flagValue)
    {
        return flagKey switch
        {
            "-alg" or "--algorithm" => new AlgorithmCommandFlag(flagKey, flagValue),
            _ => throw new FlagNotSupportedException(flagKey)
        };
    }
}