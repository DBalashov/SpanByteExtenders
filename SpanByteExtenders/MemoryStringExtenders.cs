using System.Text;

namespace SpanByteExtenders;

public static class MemoryStringExtenders
{
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

    /// <summary> Read UTF-8 string with prefixed length (1,2 or 4 bytes)  from span and advance the pointer by the number of bytes read </summary>
    /// <exception cref="ArgumentException"></exception>
    public static string ReadPrefixedString(this ref Memory<byte> mem, ReadStringPrefix prefixLength = ReadStringPrefix.Byte)
    {
        var length = prefixLength switch
                     {
                         ReadStringPrefix.Byte  => mem.Read<byte>(),
                         ReadStringPrefix.Short => mem.Read<ushort>(),
                         ReadStringPrefix.Int   => mem.Read<int>(),
                         _                      => throw new NotSupportedException(prefixLength.ToString())
                     };

        return length == 0 ? string.Empty : Encoding.UTF8.GetString(mem.Read<byte>(length));
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
        
        sourceSpan.CopyTo(mem);
        return mem.Slice(sourceSpan.Length);
    }
}