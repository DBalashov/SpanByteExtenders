using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanByteExtenders;

public static class SpanArrayExtenders
{
    public static Span<T> Read<T>(this ref Span<byte> span, int count) where T : struct => span.readItems<T>(count);

    public static Span<byte> Write<T>(this ref Span<byte> span, Span<T> items) where T : struct => span.writeItems(items);

    #region internals

    static Span<T> readItems<T>(this ref Span<byte> span, int count, int? itemSize = null) where T : struct
    {
        if (count < 0) throw new ArgumentException("Must be >0", nameof(count));

        itemSize ??= Unsafe.SizeOf<T>();

        var r = MemoryMarshal.Cast<byte, T>(span.Slice(0, count * itemSize.Value)).Slice(0, count);
        span = span.Slice(count * itemSize.Value);
        return r;
    }

    static Span<byte> writeItems<T>(this ref Span<byte> span, Span<T> items, int? count = null) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        count ??= items.Length;
        MemoryMarshal.Cast<T, byte>(items.Slice(0, count.Value)).CopyTo(span);
        return span = span.Slice(count.Value * itemSize);
    }

    #endregion

    #region Obsolete

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<Guid> ReadGuids(this ref Span<byte> span, int count) => span.Read<Guid>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<double> ReadDoubles(this ref Span<byte> span, int count) => span.Read<double>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<float> ReadFloats(this ref Span<byte> span, int count) => span.Read<float>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<decimal> ReadDecimals(this ref Span<byte> span, int count)
    {
        var r = new decimal[count];
        for (var i = 0; i < count; i++)
            r[i] = span.ReadDecimal();
        return r;
    }

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<Int64> ReadInt64s(this ref Span<byte> span, int count) => span.Read<Int64>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<UInt64> ReadUInt64s(this ref Span<byte> span, int count) => span.Read<UInt64>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<int> ReadInt32s(this ref Span<byte> span, int count) => span.Read<int>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<uint> ReadUInt32s(this ref Span<byte> span, int count) => span.Read<uint>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<short> ReadInt16s(this ref Span<byte> span, int count) => span.Read<short>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<ushort> ReadUInt16s(this ref Span<byte> span, int count) => span.Read<ushort>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<sbyte> ReadSBytes(this ref Span<byte> span, int count) => span.Read<sbyte>(count);

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Span<byte> ReadBytes(this ref Span<byte> span, int count)
    {
        var r = span.Slice(0, count);
        span = span.Slice(count);
        return r;
    }

    #endregion
}