namespace SpanByteExtenders;

public static class SpanStructExtenders
{
    public static T ReadStruct<T>(this ref Span<byte> span) where T : struct => span.Read<T>();

    public static Span<byte> WriteStruct<T>(this ref Span<byte> span, T value) where T : struct => span.Write(value);

    public static Span<T> ReadStructs<T>(this ref Span<byte> span, int count) where T : struct => span.Read<T>(count);

    public static Span<byte> WriteStructs<T>(this ref Span<byte> span, Span<T> values) where T : struct => span.Write(values);
}