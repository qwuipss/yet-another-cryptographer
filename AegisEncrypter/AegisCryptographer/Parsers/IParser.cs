using AegisCryptographer.Commands;

namespace AegisCryptographer.Parsers;

public interface IParser
{
    public ICommand ParseCommand();
}