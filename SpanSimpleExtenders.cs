namespace SpanByteExtenders;

public static class SpanSimpleExtenders
{
    public static Guid ReadGuid(this ref Span<byte> span)
    {
        var r = new Guid(span.Slice(0, 16));
        span = span.Slice(16);
        return r;
    }

    public static double ReadDouble(this ref Span<byte> span)
    {
        var r = BitConverter.ToDouble(span);
        span = span.Slice(8);
        return r;
    }

    public static float ReadFloat(this ref Span<byte> span)
    {
        var r = BitConverter.ToUInt64(span);
        span = span.Slice(4);
        return r;
    }
    
    public static decimal ReadDecimal(this ref Span<byte> span) => 
        new (span.ReadInt32s(4));

    public static Int64 ReadInt64(this ref Span<byte> span)
    {
        var r = BitConverter.ToInt64(span);
        span = span.Slice(8);
        return r;
    }

    public static UInt64 ReadUInt64(this ref Span<byte> span)
    {
        var r = BitConverter.ToUInt64(span);
        span = span.Slice(8);
        return r;
    }

    public static int ReadInt32(this ref Span<byte> span)
    {
        var r = BitConverter.ToInt32(span);
        span = span.Slice(4);
        return r;
    }

    public static uint ReadUInt32(this ref Span<byte> span)
    {
        var r = BitConverter.ToUInt32(span);
        span = span.Slice(4);
        return r;
    }

    public static short ReadInt16(this ref Span<byte> span)
    {
        var r = BitConverter.ToInt16(span);
        span = span.Slice(2);
        return r;
    }

    public static ushort ReadUInt16(this ref Span<byte> span)
    {
        var r = BitConverter.ToUInt16(span);
        span = span.Slice(2);
        return r;
    }

    public static sbyte ReadSByte(this ref Span<byte> span)
    {
        var b = span[0];
        span = span.Slice(1);
        return (sbyte) b;
    }

    public static byte ReadByte(this ref Span<byte> span)
    {
        var b = span[0];
        span = span.Slice(1);
        return b;
    }
}