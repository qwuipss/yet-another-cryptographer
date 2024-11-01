namespace Yacr.Services;

public interface ICryptoService
{
    byte[] GetRandomNonce(int size);
}