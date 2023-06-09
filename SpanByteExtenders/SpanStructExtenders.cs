﻿namespace SpanByteExtenders;

public static class SpanStructExtenders
{
    /// <summary>
    /// Read one struct and advance the pointer by the number of bytes read. Alias for <seealso cref="SpanArrayExtenders.Read{T}"/>
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static T ReadStruct<T>(this ref Span<byte> span) where T : struct => span.Read<T>();

    /// <summary>
    /// Write one struct and advance the pointer by the number of bytes written.
    /// Used Unsafe.SizeOf for calculate struct size. Alias for <seealso cref="SpanArrayExtenders.Write{T}"/>
    /// </summary>
    public static Span<byte> WriteStruct<T>(this ref Span<byte> span, T value) where T : struct => span.Write(value);

    /// <summary>
    /// Read structs and advance the pointer by the number of bytes read. Alias for <seealso cref="SpanArrayExtenders.Read{T}"/>
    /// Used Unsafe.SizeOf for calculate struct size.
    /// </summary>
    public static Span<T> ReadStructs<T>(this ref Span<byte> span, int count) where T : struct => span.Read<T>(count);

    /// <summary>
    /// Write structs and advance the pointer by the number of bytes written.
    /// Used Unsafe.SizeOf for calculate struct size. Alias for <seealso cref="SpanArrayExtenders.Write{T}"/>
    /// </summary>
    public static Span<byte> WriteStructs<T>(this ref Span<byte> span, Span<T> values) where T : struct => span.Write(values);
}