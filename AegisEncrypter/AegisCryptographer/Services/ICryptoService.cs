namespace AegisCryptographer.Services;

public interface ICryptoService
{
    byte[] GetRandomNonce(int size);
}