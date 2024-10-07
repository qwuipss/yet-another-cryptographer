using System.Text;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Commands.Resolvers.Scoped;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using static AegisCryptographer.Commands.CommandsTokens;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;
using RegexService = AegisCryptographer.Services.RegexService;

namespace AegisCryptographer.Commands.Resolvers;

public class InputCommandResolver(
    string? input,
    IRegexService regexService,
    ICommandFlagsResolver flagsResolver,
    IReader reader,
    IWriter writer) : ICommandResolver
{
    private string? Input { get; } = input;
    private IRegexService RegexService { get; } = regexService;
    private ICommandFlagsResolver FlagsResolver { get; } = flagsResolver;
    private IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;

    public ICommand Resolve()
    {
        if (Input.IsNullOrEmptyOrWhitespace()) throw new InputEmptyException();

        var splitExecutionStringInfo = RegexService.SplitExecutionStringInfo(Input!);
        var commandExecutionStringInfo = new CommandExecutionStringInfo(
            GetCommandArgumentsCollection(splitExecutionStringInfo.Arguments),
            GetCommandFlagsCollection(splitExecutionStringInfo.Flags, FlagsResolver));

        var commandToExecuteToken = commandExecutionStringInfo.CommandArgumentsCollection.Next(CommandToExecute);

        BaseScopedCommandResolver scopedResolver = commandToExecuteToken switch
        {
            EncryptLongToken or EncryptShortToken => new EncryptScopedCommandResolver(commandExecutionStringInfo, Reader, Writer),
            DecryptLongToken or DecryptShortToken => new DecryptScopedCommandResolver(commandExecutionStringInfo, Reader, Writer),
            _ => throw new CommandResolveException(commandToExecuteToken)
        };

        return scopedResolver.Resolve();
    }

    private CommandArgumentsCollection GetCommandArgumentsCollection(string commandArguments)
    {
        var splitCommandArgumentsString = RegexService.SplitCommandArgumentsString(commandArguments).ToList();
        return new CommandArgumentsCollection(splitCommandArgumentsString);
    }

    private static CommandFlagsCollection GetCommandFlagsCollection(IEnumerable<(string Flag, string Value)> flagsBundles,
        ICommandFlagsResolver flagsResolver)
    {
        var flagsCollection = new CommandFlagsCollection();

        flagsBundles.ForEach(flagBundle =>
        {
            var resolvedFlag = flagsResolver.Resolve(flagBundle.Flag, flagBundle.Value);

            if (flagsCollection.TryGetValue(resolvedFlag, out var existedFlag))
                throw new FlagDuplicateException(resolvedFlag.Key, existedFlag.Key);
        });

        return flagsCollection;
    }
}