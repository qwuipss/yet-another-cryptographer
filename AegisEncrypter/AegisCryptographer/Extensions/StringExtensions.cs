using AegisCryptographer.Exceptions;

namespace AegisCryptographer.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmptyOrWhitespace(string? str)
    {
        return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }

    public static ReadOnlySpan<byte> ToCipherKeyWithAutoPadding(this string str, int[]? paddings = null)
    {
        paddings ??= [128 / 8, 192 / 8, 256 / 8];
        var size = Settings.Encoding.GetByteCount(str);
        var paddedSize = paddings.FirstOrDefault(x => x >= size);

        if (paddedSize is 0) throw new SecretTooLongException(size, paddings.Max());

        var array = new byte[paddedSize];
        var bytes = Settings.Encoding.GetBytes(str);

        Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

        return new ReadOnlySpan<byte>(array);
    }
}