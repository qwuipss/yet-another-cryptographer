namespace AegisCryptographer.Collections;

public interface ICommandArgumentsCollection : IEnumerable<string>
{
    string Next(string expectedCommandToken);
    bool IsSealed(out List<string>? unexpectedArguments);
}