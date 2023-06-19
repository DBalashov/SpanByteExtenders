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
    /// Try to read one struct from memory and advance the pointer by the number of bytes read.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static bool TryRead<T>(this ref Memory<byte> mem, out T value) where T : struct
    {
        if (mem.Length < Unsafe.SizeOf<T>())
        {
            value = default;
            return false;
        }

        value = mem.Read<T>();
        return true;
    }

    /// <summary>
    /// Write one struct to memory and advance the pointer by the number of bytes written.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static Memory<byte> Write<T>(this ref Memory<byte> mem, T value) where T : struct
    {
        Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(mem.Span)) = value;
        return mem = mem.Slice(Unsafe.SizeOf<T>());
    }

    /// <summary>
    /// Try to write one struct to memory and advance the pointer by the number of bytes written.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static bool TryWrite<T>(this ref Memory<byte> mem, T value) where T : struct
    {
        if (mem.Length < Unsafe.SizeOf<T>()) return false;
        
        mem.Write(value);
        return true;
    }
}