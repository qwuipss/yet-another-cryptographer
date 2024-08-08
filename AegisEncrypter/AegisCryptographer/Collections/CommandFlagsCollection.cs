using AegisCryptographer.Commands.Flags;

namespace AegisCryptographer.Collections;

public class CommandFlagsCollection(IEnumerable<ICommandFlag> commandFlags) : HashSet<ICommandFlag>(commandFlags), ICommandFlagsCollection
{
}