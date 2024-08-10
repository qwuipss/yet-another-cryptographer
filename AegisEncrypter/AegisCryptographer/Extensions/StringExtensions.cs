using System.Collections.Immutable;
using System.Text;
using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Extensions;

public static class StringExtensions
{
    private static readonly ImmutableArray<int> DefaultCipherKeyPaddings = [128 / 8, 192 / 8, 256 / 8];
    
    public static bool IsNullOrEmptyOrWhitespace(string? str)
    {
        return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }

    public static string ReplaceWithOverWriting(this string str, string replaceString, int index)
    {
        return str.Remove(index, replaceString.Length).Insert(index, replaceString);
    }

    public static string WrapInQuotes(this string str)
    {
        return $"\"{str}\"";
    }

    public static ReadOnlySpan<byte> ToCipherKeyWithAutoPadding(this string str, ImmutableArray<int>? paddings = null)
    {
        paddings ??= DefaultCipherKeyPaddings;
        var size = Settings.Encoding.GetByteCount(str);
        var paddedSize = paddings.Value.FirstOrDefault(x => x >= size);

        if (paddedSize is 0) throw new SecretTooLongException(size, paddings.Max());

        var array = new byte[paddedSize];
        var bytes = Settings.Encoding.GetBytes(str);

        Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

        return new ReadOnlySpan<byte>(array);
    }
}