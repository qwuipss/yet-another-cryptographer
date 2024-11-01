namespace Yacr.Services;

public class SplitExecutionStringInfo(string arguments, IEnumerable<(string Flag, string Value)> flags) : ISplitExecutionStringInfo
{
    public string Arguments { get; } = arguments;
    public IEnumerable<(string Flag, string Value)> Flags { get; } = flags;
}