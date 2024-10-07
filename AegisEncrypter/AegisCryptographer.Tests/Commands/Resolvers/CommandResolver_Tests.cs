using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Decrypt;
using AegisCryptographer.Commands.Encrypt;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Commands.Resolvers;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using FluentAssertions;
using NSubstitute;
using static AegisCryptographer.Commands.CommandsTokens;
using static AegisCryptographer.Commands.CommandsArgumentsTokens;

namespace AegisCryptographer.Tests.Commands.Resolvers;

[TestFixture]
public class CommandResolver_Tests
{
    private ICommandFlagsResolver _flagsResolver;
    private ISplitExecutionStringInfo _splitExecutionStringInfo;
    private IRegexService _regexService;
    private IReader _reader;
    private IWriter _writer;

    [SetUp]
    public void SetUp()
    {
        _flagsResolver = Substitute.For<ICommandFlagsResolver>();
        _reader = Substitute.For<IReader>();
        _writer = Substitute.For<IWriter>();

        _reader.ReadSecret().Returns("secret");
        
        _splitExecutionStringInfo = Substitute.For<ISplitExecutionStringInfo>();
        
        _regexService = Substitute.For<IRegexService>();
        _regexService.SplitExecutionStringInfo(default!).ReturnsForAnyArgs(_splitExecutionStringInfo);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Resolve_should_throw_when_input_is_null_or_empty_or_whitespace(string? input)
    {
        Assert.Throws<InputEmptyException>(() => CreateCommandResolver(input).Resolve());
    }

    [Test]
    public void Resolve_should_resolve_encrypt_string_command()
    {
        var commands = GetInterpolatedCommandsCartesianProduct("{0} {1} " + "hello world".WrapInQuotes(),
            [[EncryptLongToken, EncryptShortToken], [StringLongToken, StringShortToken]]);

        foreach (var command in commands)
        {
            // _splitExecutionStringInfo.Arguments.Returns();
            
            var resolvedCommand = CreateCommandResolver(command).Resolve();
            
            resolvedCommand.Should().BeOfType<EncryptStringCommand>();
        }
    }

    [Test]
    public void Resolve_should_resolve_decrypt_string_command()
    {
        var commands = GetInterpolatedCommandsCartesianProduct("{0} {1} " + "hello world".WrapInQuotes(),
            [[DecryptLongToken, DecryptShortToken], [StringLongToken, StringShortToken]]);

        foreach (var resolvedCommand in commands.Select(command => CreateCommandResolver(command).Resolve()))
        {
            resolvedCommand.Should().BeOfType<DecryptStringCommand>();
        }
    }

    private static List<string> GetInterpolatedCommandsCartesianProduct(string commandToInterpolate, List<List<string>> tokens)
    {
        var commands = new List<List<string>>();
        GetCommandsCartesianProduct(commands, tokens, new string[tokens.Count]);
        var interpolatedCommands = commands.Select(command => string.Format(commandToInterpolate, [..command])).ToList();

        return interpolatedCommands;
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

    private InputCommandResolver CreateCommandResolver(string? input)
    {
        return new InputCommandResolver(input, _regexService, _flagsResolver, _reader, _writer);
    }
}