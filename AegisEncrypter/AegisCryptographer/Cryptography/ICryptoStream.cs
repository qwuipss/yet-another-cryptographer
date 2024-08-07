namespace AegisCryptographer.Cryptography;

public interface ICryptoStream
{
    public string Encrypt(string str);
    public string Decrypt(string str);
}