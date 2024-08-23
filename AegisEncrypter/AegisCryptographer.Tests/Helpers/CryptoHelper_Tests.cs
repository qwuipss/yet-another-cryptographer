using AegisCryptographer.Helpers;
using FluentAssertions;

namespace AegisCryptographer.Tests.Helpers;

public class CryptoHelper_Tests
{
    [Test]
    public void GetRandomNonce_should_generate_unique_random_nonce()
    {
        const int size = 16;

        var nonce = new List<byte[]>
        {
            CryptoHelper.GetRandomNonce(size),
            CryptoHelper.GetRandomNonce(size),
            CryptoHelper.GetRandomNonce(size)
        };
   
        Assert.That(nonce, Is.Unique);
    }
    
    [TestCase(12)]
    [TestCase(13)]
    [TestCase(14)]
    [TestCase(15)]
    [TestCase(16)]
    public void GetRandomNonce_should_generate_nonce_with_required_length(int size)
    {
        CryptoHelper.GetRandomNonce(size).Should().HaveCount(size);
    }
}