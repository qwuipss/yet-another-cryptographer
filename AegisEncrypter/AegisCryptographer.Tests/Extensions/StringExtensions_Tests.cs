using System.Text;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using FluentAssertions;

namespace AegisCryptographer.Tests.Extensions;

[TestFixture]
// ReSharper disable once InconsistentNaming
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
        str.IsNullOrEmptyOrWhitespace().Should().Be(expectedResult);
    }

    [TestCase("hello world", "мир  ", 6, "hello мир  ")]
    [TestCase("123abc", "321", 0, "321abc")]
    [TestCase("123abc", "c4", 2, "12c4bc")]
    [TestCase("123abc", "321cba", 0, "321cba")]
    public void ReplaceWithOverWriting_should_correctly_replace_string(string str, string replaceString, int index,
        string expectedString)
    {
        str.ReplaceWithOverWriting(replaceString, index).Should().Be(expectedString);
    }

    [Test]
    public void ReplaceWithOverWriting_should_throw_when_replace_string_too_long()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc123".ReplaceWithOverWriting("321", 4));
    }

    [Test]
    public void ReplaceWithOverWriting_should_throw_when_index_bigger_than_string_length()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc123".ReplaceWithOverWriting("321", 6));
    }

    [Test]
    public void ReplaceWithOverWriting_should_throw_when_index_negative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => string.Empty.ReplaceWithOverWriting(string.Empty, -1));
    }

    [TestCase("secret", new byte[] { 115, 101, 99, 114, 101, 116, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
    [TestCase("secret123456secret",
        new byte[] { 115, 101, 99, 114, 101, 116, 49, 50, 51, 52, 53, 54, 115, 101, 99, 114, 101, 116, 0, 0, 0, 0, 0, 0 })]
    [TestCase("secret123456secret123456secret",
        new byte[]
        {
            115, 101, 99, 114, 101, 116, 49, 50, 51, 52, 53, 54, 115, 101, 99, 114, 101, 116, 49, 50, 51, 52, 53, 54, 115, 101, 99, 114, 101, 116, 0, 0
        })]
    public void ToPaddedSecretKey_should_pad_key_by_default(string key, byte[] expectedKey)
    {
        var paddedSecretKey = key.ToPaddedSecretKey(Encoding.UTF8);

        paddedSecretKey.Should().BeEquivalentTo(expectedKey);
    }

    [Test]
    public void ToPaddedSecretKey_should_throw_when_secret_length_exceed_default_max_length()
    {
        Assert.Throws<SecretTooLongException>(() => new string('x', 33).ToPaddedSecretKey(Encoding.UTF8));
    }
}