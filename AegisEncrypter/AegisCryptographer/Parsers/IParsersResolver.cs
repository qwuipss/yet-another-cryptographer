namespace AegisCryptographer.Parsers;

public interface IParsersResolver
{
    public IParser Resolve(string? input);
}