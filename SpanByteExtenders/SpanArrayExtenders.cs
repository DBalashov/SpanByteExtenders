using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanByteExtenders;

public static class SpanArrayExtenders
{
    /// <summary>
    /// Read structs and advance the pointer by the number of bytes read.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    /// <param name="count">count of structs for read. If not specified - read span.Length/sizeof(T) structs </param>
    public static Span<T> Read<T>(this ref Span<byte> span, int? count = null) where T : struct
    {
        if (count < 0) throw new ArgumentException("Must be >0", nameof(count));

        var itemSize = Unsafe.SizeOf<T>();
        count ??= span.Length / itemSize;

        var lengthInBytes = count.Value * itemSize;
        var r             = MemoryMarshal.Cast<byte, T>(span.Slice(0, lengthInBytes));
        span = span.Slice(lengthInBytes);
        return r;
    }

    /// <summary>
    /// Write structs and advance the pointer by the number of bytes written.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static Span<byte> Write<T>(this ref Span<byte> span, Span<T> items) where T : struct
    {
        MemoryMarshal.Cast<T, byte>(items).CopyTo(span);
        return span = span.Slice(items.Length * Unsafe.SizeOf<T>());
    }

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