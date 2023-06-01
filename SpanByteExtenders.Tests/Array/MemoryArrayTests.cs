using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class MemoryArrayTests : BaseArrayTests
{
    protected override void run<T>(T[] rawData) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        var buff = new byte[itemSize * rawData.Length +
                            Random.Shared.Next(0, 200)]; // + some random length to buffer

        var rawDataSpan = buff.AsMemory();
        rawDataSpan.Write(rawData.AsSpan());

        var finalSpan = buff.AsMemory();
        var finalData = finalSpan.Read<T>(rawData.Length);
        Assert.IsTrue(finalData.SequenceEqual(rawData.AsSpan()));
    }
}