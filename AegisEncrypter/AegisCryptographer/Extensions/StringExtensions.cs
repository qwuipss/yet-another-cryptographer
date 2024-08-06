using System.Text;

namespace AegisCryptographer.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmptyOrWhitespace(string? str)
    {
        return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }

    public static ReadOnlySpan<byte> ToByteReadOnlySpan(this string str)
    {
        var bytes = Config.Encoding.GetBytes(str);

        return new ReadOnlySpan<byte>(bytes);
    }

    public static ReadOnlySpan<byte> ToCipherKeyWithAutoPadding(this string str)
    {
        var length = str.Length;
        var delta = 128 / 8 - length;

        byte[] array;
//todo
        if (delta < 0)
        {
            delta = 192 / 8 - length;

            if (delta < 0)
            {
                delta = 256 / 8 - length;

                if (delta < 0)
                {
                    throw new();
                }

                array = new byte[256 / 8];
            }
            else
            {
                array = new byte[192 / 8];
            }
        }
        else
        {
            array = new byte[128 / 8];
        }

        var bytes = Config.Encoding.GetBytes(str);

        Array.Copy(bytes, array, bytes.Length);

        return new ReadOnlySpan<byte>(array);
    }
}