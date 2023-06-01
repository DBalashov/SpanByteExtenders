using System.Text;

namespace SpanByteExtenders;

public static class SpanStringExtenders
{
    /// <summary> Read UTF-8 string </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadString(this ref Span<byte> span, int stringLengthInBytes) =>
        stringLengthInBytes < 0 
            ? throw new ArgumentException("Must be >0", nameof(stringLengthInBytes))
            : Encoding.UTF8.GetString(span.Read<byte>(stringLengthInBytes));

    /// <summary> Read UTF-8 string </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Span<byte> WriteString(this ref Span<byte> span, string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return span.Write(Encoding.UTF8.GetBytes(source).AsSpan());
    }

    /// <summary> Read UTF-8 string with prefixed length (1,2 or 4 bytes) </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadPrefixedString(this ref Span<byte> span, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        var length = prefixLength switch
                     {
                         ReadStringPrefix.Byte  => span.Read<byte>(),
                         ReadStringPrefix.Short => span.Read<ushort>(),
                         ReadStringPrefix.Int   => span.Read<int>(),
                         _                      => throw new NotSupportedException(prefixLength.ToString())
                     };

        return length == 0 ? string.Empty : Encoding.UTF8.GetString(span.Read<byte>(length));
    }

    /// <summary> Read UTF-8 string </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Span<byte> WritePrefixedString(this ref Span<byte> span, string source, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sourceSpan = Encoding.UTF8.GetBytes(source).AsSpan();
        switch (prefixLength)
        {
            case ReadStringPrefix.Byte:
                if (sourceSpan.Length > byte.MaxValue) throw new ArgumentException(prefixLength + " less than source length [" + sourceSpan.Length + "]");
                span.Write((byte) sourceSpan.Length);
                break;
            case ReadStringPrefix.Short:
                if (sourceSpan.Length > ushort.MaxValue) throw new ArgumentException(prefixLength + " less than source length [" + sourceSpan.Length + "]");
                span.Write((ushort) sourceSpan.Length);
                break;
            case ReadStringPrefix.Int:
                span.Write(sourceSpan.Length);
                break;
            default:
                throw new NotSupportedException(prefixLength.ToString());
        }

        return sourceSpan.Length > 0 ? span.Write(sourceSpan) : span;
    }
}

public enum ReadStringPrefix
{
    Byte  = 0,
    Short = 1,
    Int   = 2
}