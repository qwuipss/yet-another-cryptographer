using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Commands.Resolvers.Scoped;
using AegisCryptographer.Configuration;
using AegisCryptographer.Cryptography.Algorithms;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using static AegisCryptographer.Commands.CommandsTokens;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;

namespace AegisCryptographer.Commands.Resolvers;

public class CommandResolver(
    IReader reader,
    IWriter writer,
    ICryptoAlgorithmResolver cryptoAlgorithmResolver,
    ICommandFlagsResolver commandFlagsResolver,
    IRegexService regexService,
    IConfigurationProvider configurationProvider) : ICommandResolver
{
    private readonly IReader _reader  = reader;
    private readonly IWriter _writer  = writer;
    private readonly ICryptoAlgorithmResolver _cryptoAlgorithmResolver  = cryptoAlgorithmResolver;
    private readonly ICommandFlagsResolver _commandFlagsResolver  = commandFlagsResolver;
    private readonly IRegexService _regexService  = regexService;
    private readonly IConfigurationProvider _configurationProvider  = configurationProvider;

    public ICommand Resolve(string? input)
    {
        if (input.IsNullOrEmptyOrWhitespace()) throw new InputEmptyException();

        var splitExecutionStringInfo = _regexService.SplitExecutionStringInfo(input!);
        var commandExecutionStringInfo = new CommandExecutionStringInfo(
            GetCommandArgumentsCollection(splitExecutionStringInfo.Arguments),
            GetCommandFlagsCollection(splitExecutionStringInfo.Flags));

        var commandToExecuteToken = commandExecutionStringInfo.CommandArgumentsCollection.Next(CommandToExecute);

        IScopedCommandResolver scopedResolver = commandToExecuteToken switch
        {
            EncryptLongToken or EncryptShortToken => new EncryptScopedCommandResolver(_reader, _writer, _cryptoAlgorithmResolver, _regexService,
                _configurationProvider),
            DecryptLongToken or DecryptShortToken => new DecryptScopedCommandResolver(_reader, _writer, _cryptoAlgorithmResolver, _regexService,
                _configurationProvider),
            _ => throw new CommandResolveException(commandToExecuteToken)
        };

        return scopedResolver.Resolve(commandExecutionStringInfo);
    }

    private CommandArgumentsCollection GetCommandArgumentsCollection(string commandArguments)
    {
        var splitCommandArgumentsString = _regexService.SplitCommandArgumentsString(commandArguments).ToList();
        return new CommandArgumentsCollection(splitCommandArgumentsString);
    }

    private CommandFlagsCollection GetCommandFlagsCollection(IEnumerable<(string Flag, string Value)> flagsBundles)
    {
        var flagsCollection = new CommandFlagsCollection();

        foreach (var flagBundle in flagsBundles)
        {
            var resolvedCommandFlag = _commandFlagsResolver.Resolve(flagBundle.Flag, flagBundle.Value);

            if (flagsCollection.TryGetCommandFlag(resolvedCommandFlag, out var existedCommandFlag))
                throw new FlagDuplicateException(resolvedCommandFlag.Key, existedCommandFlag!.Key);

            flagsCollection.AddCommandFlag(resolvedCommandFlag);
        }

        return flagsCollection;
    }
}