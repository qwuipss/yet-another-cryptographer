namespace AegisCryptographer.Exceptions.Parsers;

public class CommandInvalidArgumentException(string argument, string command)
    : Exception($"Invalid argument \"{argument}\" for command \"{command}\"");