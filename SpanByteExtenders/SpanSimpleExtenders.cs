using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanByteExtenders;

public static class SpanSimpleExtenders
{
    public static T Read<T>(this ref Span<byte> span) where T : struct
    {
        var size = Unsafe.SizeOf<T>();

        var r = MemoryMarshal.Read<T>(span);
        span = span.Slice(size);

        return r;
    }

    public static Span<byte> Write<T>(this ref Span<byte> span, T value) where T : struct
    {
        var size = Unsafe.SizeOf<T>();

        MemoryMarshal.Write(span, ref value);
        span = span.Slice(size);

        return span;
    }

    #region Obsolete

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Guid ReadGuid(this ref Span<byte> span) => span.Read<Guid>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static double ReadDouble(this ref Span<byte> span) => span.Read<double>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static float ReadFloat(this ref Span<byte> span) => span.Read<float>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static decimal ReadDecimal(this ref Span<byte> span) => span.Read<decimal>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static Int64 ReadInt64(this ref Span<byte> span) => span.Read<Int64>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static UInt64 ReadUInt64(this ref Span<byte> span) => span.Read<UInt64>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static int ReadInt32(this ref Span<byte> span) => span.Read<int>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static uint ReadUInt32(this ref Span<byte> span) => span.Read<uint>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static short ReadInt16(this ref Span<byte> span) => span.Read<short>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static ushort ReadUInt16(this ref Span<byte> span) => span.Read<ushort>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static sbyte ReadSByte(this ref Span<byte> span) => span.Read<sbyte>();

    [Obsolete]
    [ExcludeFromCodeCoverage]
    public static byte ReadByte(this ref Span<byte> span) => span.Read<byte>();

    #endregion
}