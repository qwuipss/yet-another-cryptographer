namespace Yacr.Extensions;

public static class ObjectExtensions
{
    public static string GetTypeName(this object obj)
    {
        return obj.GetType().Name;
    }
}