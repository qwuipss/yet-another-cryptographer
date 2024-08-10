using AegisCryptographer.Commands.Flags;

namespace AegisCryptographer.Collections;

public class CommandFlagsCollection : HashSet<ICommandFlag>, ICommandFlagsCollection
{
}