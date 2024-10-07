namespace AegisCryptographer.Services;

public interface ISplitExecutionStringInfo
{
    string Arguments { get; }
    IEnumerable<(string Flag, string Value)> Flags { get; }
}