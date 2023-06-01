using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanByteExtenders;

public static class MemorySimpleExtenders
{
    /// <summary>
    /// Read one struct from span and advance the pointer by the number of bytes read.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static T Read<T>(this ref Memory<byte> mem) where T : struct
    {
        var r = Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(mem.Span));
        mem = mem.Slice(Unsafe.SizeOf<T>());
        return r;
    }

    /// <summary>
    /// Write one struct to span and advance the pointer by the number of bytes written.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static Memory<byte> Write<T>(this ref Memory<byte> mem, T value) where T : struct
    {
        Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(mem.Span)) = value;
        return mem = mem.Slice(Unsafe.SizeOf<T>());
    }
}