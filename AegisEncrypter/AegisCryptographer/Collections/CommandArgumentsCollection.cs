using System.Collections.ObjectModel;
using AegisCryptographer.Exceptions.Collections;

namespace AegisCryptographer.Collections;

public class CommandArgumentsCollection(IList<string> list)
    : ReadOnlyCollection<string>(list), ICommandArgumentsCollection
{
    private int _index;

    public string Next()
    {
        if (_index >= Count) throw new CommandArgumentMissingException();

        return base[_index++];
    }
}