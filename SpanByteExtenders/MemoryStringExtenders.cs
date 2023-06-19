using System.Text;

namespace SpanByteExtenders;

public static class MemoryStringExtenders
{
    #region Read/Write string

    /// <summary> Read UTF-8 string from span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadString(this ref Memory<byte> mem, int stringLengthInBytes) =>
        stringLengthInBytes < 0
            ? throw new ArgumentException("Must be >0", nameof(stringLengthInBytes))
            : Encoding.UTF8.GetString(mem.Read<byte>(stringLengthInBytes));

    /// <summary> Write UTF-8 string to span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Memory<byte> WriteString(this ref Memory<byte> mem, string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return mem.Write(Encoding.UTF8.GetBytes(source).AsSpan());
    }

    #endregion

    #region TryRead/TryWrite string

    public static bool TryReadString(this ref Memory<byte> mem, int stringLengthInBytes, out string value)
    {
        if (stringLengthInBytes < 0) throw new ArgumentException("Must be >0", nameof(stringLengthInBytes));
        if (mem.Length < stringLengthInBytes)
        {
            value = default!;
            return false;
        }

        value = mem.ReadString(stringLengthInBytes);
        return true;
    }

    /// <summary> Try to write UTF-8 string to span and advance the pointer by the number of bytes written. </summary>
    public static bool TryWriteString(this ref Memory<byte> mem, string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var bytesRequired = Encoding.UTF8.GetByteCount(source);
        if (mem.Length < bytesRequired) return false;
        mem.WriteString(source);
        return true;
    }

    #endregion

    #region ReadPrefixed/WritePrefixed string

    /// <summary> Read UTF-8 string with prefixed length (1,2 or 4 bytes)  from span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadPrefixedString(this ref Memory<byte> mem, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        var length = mem.Span.PeekStringLength(prefixLength);
        if (length == -1) throw new ArgumentException("Can't read string length from span");

        mem = mem.Slice(1 << (int) prefixLength);
        return length == 0 ? string.Empty : mem.ReadString(length);
    }

    /// <summary> Read UTF-8 string to span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Memory<byte> WritePrefixedString(this ref Memory<byte> mem, string source, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sourceSpan = Encoding.UTF8.GetBytes(source);
        switch (prefixLength)
        {
            case ReadStringPrefix.Byte:
                if (sourceSpan.Length > byte.MaxValue) throw new ArgumentException(prefixLength + " less than source length [" + sourceSpan.Length + "]");
                mem.Write((byte) sourceSpan.Length);
                break;
            case ReadStringPrefix.Short:
                if (sourceSpan.Length > ushort.MaxValue) throw new ArgumentException(prefixLength + " less than source length [" + sourceSpan.Length + "]");
                mem.Write((ushort) sourceSpan.Length);
                break;
            case ReadStringPrefix.Int:
                mem.Write(sourceSpan.Length);
                break;
            default:
                throw new NotSupportedException(prefixLength.ToString());
        }

        if (sourceSpan.Length == 0) return mem;

        mem.WriteString(source);
        return mem;
    }

    #endregion

    #region TryReadPrefixed/TryWritePrefixed string

    public static bool TryReadPrefixedString(this ref Memory<byte> mem, ReadStringPrefix prefixLength, out string value)
    {
        value = default!;
        var length = mem.Span.PeekStringLength(prefixLength);
        if (length < 0) return false;

        var prefixLengthInBytes = 1 << (int) prefixLength;
        if (mem.Length < length + prefixLengthInBytes) return false;

        mem   = mem.Slice(prefixLengthInBytes);
        value = mem.ReadString(length);
        return true;
    }

    public static bool TryWritePrefixedString(this ref Memory<byte> mem, string source, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        ArgumentNullException.ThrowIfNull(source);

        var prefixLengthBytes = 1 << (int) prefixLength;
        if (mem.Length < prefixLengthBytes + Encoding.UTF8.GetByteCount(source)) return false;

        mem.WritePrefixedString(source, prefixLength);
        return true;
    }

    #endregion
}