namespace AegisCryptographer.Exceptions.Parsers;

public class ParserResolveException(string command) : Exception($"Unable to resolve command: \"{command}\".");