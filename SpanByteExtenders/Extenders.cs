namespace SpanByteExtenders;

static class Extenders
{
    public static int PeekStringLength(this Span<byte> span, ReadStringPrefix prefixLength) =>
        prefixLength switch
        {
            ReadStringPrefix.Byte  => span.Length >= 1 ? span[0] : -1,
            ReadStringPrefix.Short => span.Length >= 2 ? BitConverter.ToUInt16(span) : -1,
            ReadStringPrefix.Int   => span.Length >= 4 ? BitConverter.ToInt32(span) : -1,
            _                      => throw new NotSupportedException(prefixLength.ToString())
        };
}