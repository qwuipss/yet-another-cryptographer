namespace AegisCryptographer.Collections;

public interface ICommandArgumentsCollection
{
    public string this[Range range] { get; }
    public string this[int index] { get; }
}