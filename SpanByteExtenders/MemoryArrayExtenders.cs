using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanByteExtenders;

public static class MemoryArrayExtenders
{
    /// <summary>
    /// Read structs and advance the pointer by the number of bytes read.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    /// <param name="count">count of structs for read. If not specified - read span.Length/sizeof(T) structs </param>
    public static Span<T> Read<T>(this ref Memory<byte> mem, int? count = null) where T : struct
    {
        if (count < 0) throw new ArgumentException("Must be >0", nameof(count));

        var itemSize = Unsafe.SizeOf<T>();
        count ??= mem.Length / itemSize;

        var bytes = count.Value * itemSize;
        
        var r     = MemoryMarshal.Cast<byte, T>(mem.Span.Slice(0, bytes));
        mem = mem.Slice(bytes);
        return r;
    }

    /// <summary>
    /// Write structs and advance the pointer by the number of bytes written.
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static Memory<byte> Write<T>(this ref Memory<byte> mem, Span<T> items) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        MemoryMarshal.Cast<T, byte>(items.Slice(0, items.Length)).CopyTo(mem.Span);
        return mem = mem.Slice(items.Length * itemSize);
    }
}