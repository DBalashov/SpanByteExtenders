using System.Runtime.InteropServices;

namespace SpanByteExtenders;

public static class SpanArrayExtenders
{
    public static Span<T> readItems<T>(this ref Span<byte> span, int count, int itemSize) where T : struct
    {
        if (count < 0) throw new ArgumentException("Must be >0", nameof(count));

        var r = MemoryMarshal.Cast<byte, T>(span.Slice(count * itemSize));
        span = span.Slice(count * itemSize);
        return r;
    }

    public static Span<Guid> ReadGuids(this ref Span<byte> span, int count) => span.readItems<Guid>(count, 16);

    public static Span<double> ReadDoubles(this ref Span<byte> span, int count) => span.readItems<double>(count, 8);

    public static Span<float> ReadFloats(this ref Span<byte> span, int count) => span.readItems<float>(count, 4);

    public static Span<decimal> ReadDecimals(this ref Span<byte> span, int count)
    {
        var r = new decimal[count];
        for (var i = 0; i < count; i++)
            r[i] = span.ReadDecimal();
        return r;
    }

    public static Span<Int64> ReadInt64s(this ref Span<byte> span, int count) => span.readItems<Int64>(count, 8);

    public static Span<UInt64> ReadUInt64s(this ref Span<byte> span, int count) => span.readItems<UInt64>(count, 8);

    public static Span<int> ReadInt32s(this ref Span<byte> span, int count) => span.readItems<int>(count, 4);

    public static Span<uint> ReadUInt32s(this ref Span<byte> span, int count) => span.readItems<uint>(count, 4);

    public static Span<short> ReadInt16s(this ref Span<byte> span, int count) => span.readItems<short>(count, 2);

    public static Span<ushort> ReadUInt16s(this ref Span<byte> span, int count) => span.readItems<ushort>(count, 2);

    public static Span<sbyte> ReadSBytes(this ref Span<byte> span, int count) => span.readItems<sbyte>(count, 1);

    public static Span<byte> ReadBytes(this ref Span<byte> span, int count)
    {
        var r = span.Slice(0, count);
        span = span.Slice(count);
        return r;
    }
}