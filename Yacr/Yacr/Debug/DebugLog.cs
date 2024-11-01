namespace Yacr.Debug;

public class DebugLog(string context = "Program.Main") : IDebugLog
{
    private readonly string _context = context;

    public void Info(string message)
    {
        System.Diagnostics.Debug.WriteLine($"[{_context}] {message}");
    }
}