namespace Yacr.Cryptography;

public interface ICryptoStream
{
    string Encrypt(string str);
    string Decrypt(string str);
}