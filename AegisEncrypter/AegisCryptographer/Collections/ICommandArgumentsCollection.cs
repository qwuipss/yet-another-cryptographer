namespace AegisCryptographer.Collections;

public interface ICommandArgumentsCollection : IEnumerable<string>
{
    public string Next(string expectedCommandToken);
    public bool IsSealed(out List<string>? unexpectedArguments);
}