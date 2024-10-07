using AegisCryptographer.Services;
using FluentAssertions;

namespace AegisCryptographer.Tests.Helpers;

[TestFixture]
public class CryptoService_Tests
{
    private CryptoService _cryptoService;

    [SetUp]
    public void SetUp()
    {
        _cryptoService = new CryptoService();
    }
    
    [Test]
    public void GetRandomNonce_should_generate_unique_random_nonce()
    {
        const int size = 16;

        var nonce = new List<byte[]>
        {
            _cryptoService.GetRandomNonce(size),
            _cryptoService.GetRandomNonce(size),
            _cryptoService.GetRandomNonce(size)
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
        _cryptoService.GetRandomNonce(size).Should().HaveCount(size);
    }
}