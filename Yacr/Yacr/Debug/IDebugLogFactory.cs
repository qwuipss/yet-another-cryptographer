namespace Yacr.Debug;

public interface IDebugLogFactory
{
    IDebugLog ForContext<T>();
}