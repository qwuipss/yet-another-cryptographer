using System.Collections.ObjectModel;
using Yacr.Exceptions;

namespace Yacr.Collections;

public class CommandArgumentsCollection(IList<string> list) : ReadOnlyCollection<string>(list), ICommandArgumentsCollection
{
    private int _index;

    public string Next(string expectedCommandToken)
    {
        if (_index >= Count) throw new CommandArgumentMissingException(expectedCommandToken);

        return base[_index++];
    }

    public bool IsSealed(out List<string>? unexpectedArguments)
    {
        if (_index == Count)
        {
            unexpectedArguments = null;
            return true;
        }

        unexpectedArguments = this.Where((_, i) => i >= _index).ToList();
        return false;
    }
}