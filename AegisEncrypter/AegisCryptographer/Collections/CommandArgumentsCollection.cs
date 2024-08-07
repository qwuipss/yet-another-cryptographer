using System.Collections.ObjectModel;
using AegisCryptographer.Collections.Exceptions;

namespace AegisCryptographer.Collections;

public class CommandArgumentsCollection(IList<string> list) : ReadOnlyCollection<string>(list)
{
    public string this[Range range]
    {
        get { return string.Join(" ", this.Where((_, i) => i >= range.Start.Value).Select(x => x)); }
    }

    public new string this[int index]
    {
        get
        {
            if (index >= Count) throw new CommandArgumentMissingException();

            return base[index];
        }
    }
}