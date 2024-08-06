using AegisCryptographer.IO;

namespace AegisCryptographer.Runners;

public class LoopRunner(IReader reader, IWriter writer) : BaseRunner(reader, writer)
{
    public override void Run()
    {
        while (true)
        {
            var input = Reader.ReadLine();

            if (input is "q" or "quit" or "ex" or "exit")
            {
                break;
            }

            RunInput(input);
        }
    }
}