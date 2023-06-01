using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class MemorySimpleTests : BaseSimpleTests
{
    protected override void run<T>(T value) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        var buff = new byte[itemSize +
                            Random.Shared.Next(0, 200)]; // + some random length to buffer

        var rawDataSpan = buff.AsMemory();
        rawDataSpan.Write(value);

        var finalSpan = buff.AsMemory();
        var finalData = finalSpan.Read<T>();
        Assert.IsTrue(Equals(value, finalData));
    }
}