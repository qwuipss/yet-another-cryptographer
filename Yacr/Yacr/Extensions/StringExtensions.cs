using System.Collections.Immutable;
using System.Text;
using Yacr.Exceptions;

namespace Yacr.Extensions;

public static class StringExtensions
{
    private static readonly ImmutableArray<int> DefaultCipherKeyPaddings = [128 / 8, 192 / 8, 256 / 8];

    public static bool IsNullOrEmptyOrWhitespace(this string? str)
    {
        return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }

    public static string ReplaceWithOverWriting(this string str, string replaceString, int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Value cannot be negative.");

        if (index >= str.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Value cannot be more than string length.");

        if (str.Length - index < replaceString.Length)
            throw new ArgumentOutOfRangeException(nameof(replaceString), "Replace string is too long.");

        return str.Remove(index, replaceString.Length).Insert(index, replaceString);
    }

    public static string WrapInQuotes(this string str)
    {
        return $"\"{str}\"";
    }

    public static byte[] ToPaddedSecretKey(this string str, Encoding encoding)
    {
        var size = encoding.GetByteCount(str);
        var paddedSize = DefaultCipherKeyPaddings.FirstOrDefault(x => x >= size);

        if (paddedSize is 0) throw new SecretTooLongException(size, DefaultCipherKeyPaddings.Max());

        var array = new byte[paddedSize];
        var bytes = encoding.GetBytes(str);

        Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

        return array;
    }
}