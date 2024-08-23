using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using FluentAssertions;
using static AegisCryptographer.Commands.CommandsTokens;

namespace AegisCryptographer.Tests.Extensions;

public class StringExtensions_Tests
{
    [TestCase(null, true)]
    [TestCase("", true)]
    [TestCase(" ", true)]
    [TestCase("   ", true)]
    [TestCase("\t", true)]
    [TestCase("\t\t", true)]
    [TestCase("x", false)]
    [TestCase("   x", false)]
    [TestCase("   x   ", false)]
    [TestCase("   ъ   ", false)]
    public void IsNullOrEmptyOrWhiteSpace_should_correctly_define_string(string? str, bool expectedResult)
    {
        StringExtensions.IsNullOrEmptyOrWhitespace(str).Should().Be(expectedResult);
    }

    [TestCase("hello world", "мир  ", 6, "hello мир  ")]
    [TestCase("123abc", "321", 0, "321abc")]
    [TestCase("123abc", "c4", 2, "12c4bc")]
    [TestCase("123abc", "321cba", 0, "321cba")]
    public void ReplaceWithOverWriting_should_correctly_replace_string(string str, string replaceString, int index,
        string expectedString)
    {
        StringExtensions.ReplaceWithOverWriting(str, replaceString, index).Should().Be(expectedString);
    }

    [Test]
    public void ReplaceWithOverWriting_should_throw_when_replace_string_too_long()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            StringExtensions.ReplaceWithOverWriting("abc123", "321", 4);
        });
    }

    [Test]
    public void ReplaceWithOverWriting_should_throw_when_index_bigger_than_string_length()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            StringExtensions.ReplaceWithOverWriting("abc123", "321", 6);
        });
    }

    [Test]
    public void ReplaceWithOverWriting_should_throw_when_index_negative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            StringExtensions.ReplaceWithOverWriting(string.Empty, string.Empty, -1);
        });
    }

    [TestCase("secret", new byte[] { 115, 101, 99, 114, 101, 116, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
    [TestCase("secret123456secret",
        new byte[]
        {
            115, 101, 99, 114, 101, 116, 49, 50, 51, 52, 53, 54, 115, 101, 99, 114, 101, 116, 0, 0, 0, 0, 0, 0
        })]
    [TestCase("secret123456secret123456secret",
        new byte[]
        {
            115, 101, 99, 114, 101, 116, 49, 50, 51, 52, 53, 54, 115, 101, 99, 114, 101, 116, 49, 50, 51, 52, 53, 54,
            115, 101, 99, 114, 101, 116, 0, 0
        })]
    public void ToPaddedSecretKey_should_pad_key_by_default(string key, byte[] expectedKey)
    {
        var paddedSecretKey = StringExtensions.ToPaddedSecretKey(key);

        paddedSecretKey.Should().BeEquivalentTo(expectedKey);
    }

    [Test]
    public void ToPaddedSecretKey_should_throw_when_secret_length_exceed_default_max_length()
    {
        Assert.Throws<SecretTooLongException>(() => { StringExtensions.ToPaddedSecretKey(new string('x', 33)); });
    }

    // [TestCaseSource(typeof(TestCases), nameof(TestCases.ToCommandExecutionStringInfo))]
    // public void ToCommandExecutionStringInfo_should_correctly_parse_string(
    //     (string Data, CommandExecutionStringInfo ExpectedStringInfo) bundle)
    // {
    //     var executionStringInfo = StringExtensions.ToCommandExecutionStringInfo(bundle.Data, new CommandFlagsResolver());
    //
    //     executionStringInfo.Should().BeEquivalentTo(bundle.ExpectedStringInfo);
    // }
    //
    // private class TestCases
    // {
    //     public static IEnumerable<(string, CommandExecutionStringInfo)> ToCommandExecutionStringInfo
    //     {
    //         get
    //         {
    //             yield return new($"encrypt string {"hello world".WrapInQuotes()}", new CommandExecutionStringInfo(
    //                 new CommandArgumentsCollection(["encrypt", "string", "hello world".WrapInQuotes()]),
    //                 new CommandFlagsCollection()));
    //             yield return new($"encrypt string {"hello world".WrapInQuotes()} -alg aesgcm", new CommandExecutionStringInfo(
    //                 new CommandArgumentsCollection(["encrypt", "string", "hello world".WrapInQuotes()]),
    //                 new CommandFlagsCollection{new AlgorithmCommandFlag("-a", "aesgcm")}));
    //         }
    //     }
    // }
}