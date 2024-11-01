namespace Yacr.Debug;

public class DebugLogFactory : IDebugLogFactory
{
    public IDebugLog ForContext<T>()
    {
        return new DebugLog(typeof(T).Name);
    }
}