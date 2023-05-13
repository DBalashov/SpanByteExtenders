using System.Text;

namespace SpanByteExtenders;

public static class SpanStringExtenders
{
    /// <summary> Read UTF-8 string </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadString(this ref Span<byte> span, int stringLengthInBytes)
    {
        if (stringLengthInBytes < 0) throw new ArgumentException("Must be >0", nameof(stringLengthInBytes));

        var r = Encoding.UTF8.GetString(span.Slice(0, stringLengthInBytes));
        span = span.Slice(stringLengthInBytes);

        return r;
    }

    /// <summary> Read UTF-8 string with prefixed length (1,2 or 4 bytes) </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadPrefixedString(this ref Span<byte> span, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        var length = prefixLength switch
                     {
                         ReadStringPrefix.Byte  => span.ReadByte(),
                         ReadStringPrefix.Short => span.ReadInt16(),
                         ReadStringPrefix.Int   => span.ReadInt32(),
                         _                      => throw new NotSupportedException(prefixLength.ToString())
                     };

        if (length == 0) return string.Empty;

        var r = Encoding.UTF8.GetString(span.Slice(0, length));
        span = span.Slice(length);

        return r;
    }
}

public enum ReadStringPrefix
{
    Byte  = 0,
    Short = 1,
    Int   = 2
}