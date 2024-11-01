using Yacr.Extensions;
using Yacr.Collections;
using Yacr.Commands.Flags;
using Yacr.Commands.Resolvers.Scoped;
using Yacr.Configuration;
using Yacr.Cryptography.Algorithms;
using Yacr.Debug;
using Yacr.Exceptions;
using Yacr.IO;
using Yacr.Services;
using static Yacr.Commands.CommandsTokens;
using static Yacr.Commands.ExpectedCommandsTokens;

namespace Yacr.Commands.Resolvers;

public class CommandResolver(
    IReader reader,
    IWriter writer,
    IDebugLogFactory debugLogFactory,
    ICryptoAlgorithmResolver cryptoAlgorithmResolver,
    ICommandFlagsResolver commandFlagsResolver,
    IRegexService regexService,
    IConfigurationProvider configurationProvider) : ICommandResolver
{
    private readonly IReader _reader = reader;
    private readonly IWriter _writer = writer;
    private readonly IDebugLog _debugLog = debugLogFactory.ForContext<CommandResolver>();
    private readonly ICryptoAlgorithmResolver _cryptoAlgorithmResolver = cryptoAlgorithmResolver;
    private readonly ICommandFlagsResolver _commandFlagsResolver = commandFlagsResolver;
    private readonly IRegexService _regexService = regexService;
    private readonly IConfigurationProvider _configurationProvider = configurationProvider;

    public ICommand Resolve(string? input)
    {
        if (input.IsNullOrEmptyOrWhitespace())
        {
            DebugLogEmptyInputReceived();
            throw new InputEmptyException();
        }

        var splitExecutionStringInfo = _regexService.SplitExecutionStringInfo(input!);
        var commandExecutionStringInfo = new CommandExecutionStringInfo(
            GetCommandArgumentsCollection(splitExecutionStringInfo.Arguments),
            GetCommandFlagsCollection(splitExecutionStringInfo.Flags));

        var commandToExecuteToken = commandExecutionStringInfo.CommandArgumentsCollection.Next(CommandToExecute);

        IScopedCommandResolver scopedCommandResolver = commandToExecuteToken switch
        {
            EncryptLongToken or EncryptShortToken => new EncryptScopedCommandResolver(_reader, _writer, _cryptoAlgorithmResolver, _regexService,
                _configurationProvider),
            DecryptLongToken or DecryptShortToken => new DecryptScopedCommandResolver(_reader, _writer, _cryptoAlgorithmResolver, _regexService,
                _configurationProvider),
            _ => throw new CommandResolveException(commandToExecuteToken)
        };

        DebugLogScopedCommandResolverResolved(scopedCommandResolver);
        var resolvedCommand = scopedCommandResolver.Resolve(commandExecutionStringInfo);

        return resolvedCommand;
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
            {
                DebugLogFlagDuplicateDetected(resolvedCommandFlag, existedCommandFlag!);   
                throw new FlagDuplicateException(resolvedCommandFlag.Key, existedCommandFlag!.Key);
            }

            flagsCollection.AddCommandFlag(resolvedCommandFlag);
        }

        return flagsCollection;
    }

    #region DebugLog

    private void DebugLogEmptyInputReceived()
    {
        _debugLog.Info($"Empty input received. Throwing {nameof(InputEmptyException)}.");
    }
    
    private void DebugLogScopedCommandResolverResolved(IScopedCommandResolver scopedCommandResolver)
    {
        _debugLog.Info($"Scoped command resolver resolved: {scopedCommandResolver.GetTypeName()}");
    }

    private void DebugLogFlagDuplicateDetected(ICommandFlag resolvedCommandFlag, ICommandFlag existedCommandFlag)
    {
        _debugLog.Info($"Flag duplicate detected. [{resolvedCommandFlag}] conflicts with [{existedCommandFlag}]. Throwing {nameof(FlagDuplicateException)}.");
    }

    #endregion
}