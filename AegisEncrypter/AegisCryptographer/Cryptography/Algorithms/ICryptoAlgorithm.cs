namespace AegisCryptographer.Cryptography.Algorithms;

public interface ICryptoAlgorithm
{
    public byte[] Encrypt(byte[] data);
    public byte[] Decrypt(byte[] data);
}