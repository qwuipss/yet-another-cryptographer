using Yacr.Extensions;

namespace Yacr.Exceptions;

public class CommandResolveException(string command)
    : IntentionalException($"Unable to resolve command {command.WrapInQuotes()}.");