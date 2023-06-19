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
    /// Try to read structs and advance the pointer by the number of bytes read.
    /// if span.Length less than count of bytes for read - return false and do not change span
    /// </summary>
    /// <returns>true if read successful, false otherwise</returns>
    public static bool TryRead<T>(this ref Memory<byte> mem, out Span<T> value, int? count = null) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        count ??= mem.Length / itemSize;

        var lengthInBytes = count.Value * itemSize;
        if (mem.Length < lengthInBytes)
        {
            value = default;
            return false;
        }

        value = MemoryMarshal.Cast<byte, T>(mem.Span.Slice(0, lengthInBytes));
        mem  = mem.Slice(lengthInBytes);
        return true;
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
    
    /// <summary>
    /// Try to write structs and advance the pointer by the number of bytes written.
    /// if span.Length less than count of bytes for write - return false and do not change span
    /// </summary>
    /// <returns>true if write successful, false otherwise</returns>
    public static bool TryWrite<T>(this ref Memory<byte> mem, Span<T> items) where T : struct
    {
        var itemSize      = Unsafe.SizeOf<T>();
        var lengthInBytes = items.Length * itemSize;
        if (mem.Length < lengthInBytes) return false;
        
        MemoryMarshal.Cast<T, byte>(items).CopyTo(mem.Span);
        mem = mem.Slice(lengthInBytes);
        return true;
    }
}