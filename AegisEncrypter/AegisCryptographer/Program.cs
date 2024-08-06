using System.Text;
using AegisCryptographer.IO;
using AegisCryptographer.Runners;

namespace AegisCryptographer;

public class Program
{
    public static void Main(string[] args)
    {
        var writer = new Writer();
        var reader = new Reader(writer);

        if (args.Length is 0)
        {
            new LoopRunner(reader, writer).Run();
        }

        // todo cli runner
        throw new NotImplementedException();
    }
}