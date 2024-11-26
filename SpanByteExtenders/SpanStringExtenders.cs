using System.Text;

namespace SpanByteExtenders;

public static class SpanStringExtenders
{
    #region Read/Write string

    /// <summary> Read UTF-8 string from span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadString(this ref Span<byte> span, int stringLengthInBytes) =>
        stringLengthInBytes < 0
            ? throw new ArgumentException("Must be >0", nameof(stringLengthInBytes))
            : Encoding.UTF8.GetString(span.Read<byte>(stringLengthInBytes));

    /// <summary> Write UTF-8 string to span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Span<byte> WriteString(this ref Span<byte> span, string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return span.Write(Encoding.UTF8.GetBytes(source).AsSpan());
    }

    #endregion

    #region TryRead/TryWrite string

    public static bool TryReadString(this ref Span<byte> span, int stringLengthInBytes, out string value)
    {
        if (stringLengthInBytes < 0) throw new ArgumentException("Must be >0", nameof(stringLengthInBytes));
        if (span.Length < stringLengthInBytes)
        {
            value = default!;
            return false;
        }

        value = span.ReadString(stringLengthInBytes);
        return true;
    }

    /// <summary> Try to write UTF-8 string to span and advance the pointer by the number of bytes written. </summary>
    public static bool TryWriteString(this ref Span<byte> span, string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var bytesRequired = Encoding.UTF8.GetByteCount(source);
        if (span.Length < bytesRequired) return false;
        span.WriteString(source);
        return true;
    }

    #endregion

    #region ReadPrefixed/WritePrefixed string with prefixed length

    /// <summary> Read UTF-8 string with prefixed length (1,2 or 4 bytes)  from span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadPrefixedString(this ref Span<byte> span, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        var length = span.PeekStringLength(prefixLength);
        if (length == -1) throw new ArgumentException("Can't read string length from span");

        span = span.Slice(1 << (int) prefixLength);
        return length == 0 ? string.Empty : Encoding.UTF8.GetString(span.Read<byte>(length));
    }

    /// <summary> Read UTF-8 string to span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Span<byte> WritePrefixedString(this ref Span<byte> span, string source, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sourceSpan = Encoding.UTF8.GetBytes(source);
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

        if (sourceSpan.Length == 0) return span;

        sourceSpan.CopyTo(span);
        return span = span.Slice(sourceSpan.Length);
    }

    #endregion

    #region TryReadPrefixed/TryWritePrefixed string

    public static bool TryReadPrefixedString(this ref Span<byte> span, ReadStringPrefix prefixLength, out string value)
    {
        value = default!;
        var length = span.PeekStringLength(prefixLength);
        if (length < 0) return false;

        var prefixLengthInBytes = 1 << (int) prefixLength;
        if (span.Length < length + prefixLengthInBytes) return false;

        span  = span.Slice(prefixLengthInBytes);
        value = span.ReadString(length);
        return true;
    }

    public static bool TryWritePrefixedString(this ref Span<byte> span, string source, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        ArgumentNullException.ThrowIfNull(source);

        var prefixLengthBytes = 1 << (int) prefixLength;
        if (span.Length < prefixLengthBytes + Encoding.UTF8.GetByteCount(source)) return false;

        span.WritePrefixedString(source, prefixLength);
        return true;
    }

    #endregion
}

public enum ReadStringPrefix
{
    Byte  = 0,
    Short = 1,
    Int   = 2
}