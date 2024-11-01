using System.Text;
using Yacr.Commands.Decrypt;
using Yacr.Commands.Encrypt;
using Yacr.Commands.Flags;
using Yacr.Commands.Resolvers;
using Yacr.Configuration;
using Yacr.Cryptography.Algorithms;
using Yacr.Debug;
using Yacr.Exceptions;
using Yacr.Extensions;
using Yacr.IO;
using Yacr.Services;
using FluentAssertions;
using NSubstitute;
using static Yacr.Commands.CommandsTokens;
using static Yacr.Commands.CommandsArgumentsTokens;

namespace Yacr.Tests.Commands.Resolvers;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class CommandResolver_Tests
{
    private IReader _reader;
    private IWriter _writer;
    private IDebugLogFactory _debugLogFactory;
    private ICryptoAlgorithmResolver _cryptoAlgorithmResolver;
    private ICommandFlagsResolver _commandFlagsResolver;
    private IRegexService _regexService;
    private IConfigurationProvider _configurationProvider;
    private ISplitExecutionStringInfo _splitExecutionStringInfo;

    [SetUp]
    public void SetUp()
    {
        _reader = Substitute.For<IReader>();
        _reader.ReadSecret().Returns("secret");

        _writer = Substitute.For<IWriter>();

        _debugLogFactory = Substitute.For<IDebugLogFactory>();
        // _debugLogFactory.ForContext<object>().ReturnsForAnyArgs(Substitute.For<IDebugLog>());

        _cryptoAlgorithmResolver = Substitute.For<ICryptoAlgorithmResolver>();
        
        _commandFlagsResolver = Substitute.For<ICommandFlagsResolver>();

        _regexService = Substitute.For<IRegexService>();
        _regexService.SplitExecutionStringInfo(default!).ReturnsForAnyArgs(_splitExecutionStringInfo);

        _configurationProvider = Substitute.For<IConfigurationProvider>();
        _configurationProvider.Encoding.ReturnsForAnyArgs(Encoding.UTF8);
        
        _splitExecutionStringInfo = Substitute.For<ISplitExecutionStringInfo>();
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Resolve_should_throw_when_input_is_null_or_empty_or_whitespace(string? input)
    {
        Assert.Throws<InputEmptyException>(() => CreateCommandResolver().Resolve(input));
    }

    [TestCase("fake command \"fake arg\"")]
    public void Resolve_should_throw_when_unable_resolve_command(string input)
    {
        _splitExecutionStringInfo.Arguments.ReturnsForAnyArgs(input);
        _regexService.SplitExecutionStringInfo(input).ReturnsForAnyArgs(_splitExecutionStringInfo);
        _regexService.SplitCommandArgumentsString(input).Returns(input.Split());

        Assert.Throws<CommandResolveException>(() => CreateCommandResolver().Resolve(input));
    }

    [Test]
    public void Resolve_should_resolve_encrypt_string_command()
    {
        TestCore_Resolve_should_resolve_command<EncryptStringCommand>([
            [EncryptLongToken, EncryptShortToken], [StringLongToken, StringShortToken], ["hello world".WrapInQuotes()]
        ]);
    }

    [Test]
    public void Resolve_should_resolve_decrypt_string_command()
    {
        TestCore_Resolve_should_resolve_command<DecryptStringCommand>([
            [DecryptLongToken, DecryptShortToken], [StringLongToken, StringShortToken], ["hello world".WrapInQuotes()]
        ]);
    }

    private void TestCore_Resolve_should_resolve_command<TCommand>(List<List<string>> tokens)
    {
        var splitCommands = GetCommandsCartesianProduct(tokens);

        foreach (var splitCommand in splitCommands)
        {
            var command = string.Join(' ', splitCommand);
            _splitExecutionStringInfo.Arguments.ReturnsForAnyArgs(command);
            _regexService.SplitExecutionStringInfo(command).ReturnsForAnyArgs(_splitExecutionStringInfo);
            _regexService.SplitCommandArgumentsString(command).Returns(splitCommand);
            _regexService.GetQuotesStringWithEscapedQuotes(default!).ReturnsForAnyArgs(callInfo => callInfo.Arg<string>());

            var resolvedCommand = CreateCommandResolver().Resolve(command);

            resolvedCommand.Should().BeOfType<TCommand>();
        }
    }

    private static List<List<string>> GetCommandsCartesianProduct(List<List<string>> tokens)
    {
        var commands = new List<List<string>>();
        GetCommandsCartesianProduct(commands, tokens, new string[tokens.Count]);
        return commands;
    }

    private static void GetCommandsCartesianProduct(List<List<string>> commands, List<List<string>> tokens, string[] results, int index = 0)
    {
        if (index == tokens.Count)
        {
            commands.Add([..results]);
            return;
        }

        for (int i = 0; i < tokens[index].Count; i++)
        {
            results[index] = tokens[index][i];
            GetCommandsCartesianProduct(commands, tokens, results, index + 1);
        }
    }

    private CommandResolver CreateCommandResolver()
    {
        return new CommandResolver(_reader, _writer, _debugLogFactory, _cryptoAlgorithmResolver, _commandFlagsResolver, _regexService, _configurationProvider);
    }
}