using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanByteExtenders;

public static class SpanStructExtenders
{
    public static T ReadStruct<T>(this ref Span<byte> span) where T : struct
    {
        var size = Unsafe.SizeOf<T>();

        var r = MemoryMarshal.Read<T>(span);
        span = span.Slice(size);

        return r;
    }

    public static Span<T> ReadStructs<T>(this ref Span<byte> span, int count) where T : struct
    {
        if (count < 0) throw new ArgumentException("Must be >0", nameof(count));

        var size = Unsafe.SizeOf<T>();

        var r = MemoryMarshal.Cast<byte, T>(span).Slice(0, count);
        span = span.Slice(size * count);

        return r;
    }
}