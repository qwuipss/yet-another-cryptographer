namespace AegisCryptographer.Helpers;

public static class PathHelper
{
    public static bool IsFullPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
            !Path.IsPathRooted(path))
            return false;

        var pathRoot = Path.GetPathRoot(path);

        if (pathRoot is null) return false;

        if (pathRoot.Length <= 2 &&
            pathRoot != "/") // Accepts X:\ and \\UNC\PATH, rejects empty string, \ and X:, but accepts / to support Linux
            return false;

        if (pathRoot[0] != '\\' || pathRoot[1] != '\\')
            return true; // Rooted and not a UNC path

        return
            pathRoot.Trim('\\')
                .Contains('\\'); // A UNC server name without a share name (e.g "\\NAME" or "\\NAME\") is invalid
    }
}