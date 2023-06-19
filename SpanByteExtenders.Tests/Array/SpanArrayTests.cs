using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class ArrayTests : BaseArrayTests
{
    protected override void run<T>(T[] rawData) where T : struct
    {
        runSimple(rawData);
        runTry(rawData);
    }

    void runSimple<T>(T[] rawData) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        var buff = new byte[itemSize * rawData.Length +
                            Random.Shared.Next(0, 200)]; // + some random length to buffer

        var rawDataSpan = buff.AsSpan();
        rawDataSpan.Write(rawData.AsSpan());

        var finalSpan = buff.AsSpan();
        var finalData = finalSpan.Read<T>(rawData.Length);
        Assert.IsTrue(finalData.SequenceEqual(rawData.AsSpan()));
    }

    void runTry<T>(T[] rawData) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        var buff     = new byte[itemSize * rawData.Length - 1]; // buffer less than needed

        var rawDataSpan   = buff.AsSpan();
        var initialLength = rawDataSpan.Length;
        var resultWrite   = rawDataSpan.TryWrite(rawData.AsSpan());
        Assert.IsFalse(resultWrite);
        Assert.IsTrue(rawDataSpan.Length == initialLength); // target span must not be changed

        var finalSpan  = buff.AsSpan();
        var resultRead = finalSpan.TryRead<T>(out var finalData, rawData.Length);
        Assert.IsFalse(resultRead);
        Assert.IsTrue(finalSpan.Length == initialLength); // target span must not be changed
    }
}