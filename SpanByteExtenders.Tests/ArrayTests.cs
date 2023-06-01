using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class ArrayTests
{
    const int SIZE = 16;

    void run<T>(T[] rawData) where T : struct
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

    [Test]
    public void TestGuids() => run(Enumerable.Range(0, SIZE).Select(p => Guid.NewGuid()).ToArray());

    [Test]
    public void TestDoubles() => run(Enumerable.Range(0, SIZE).Select(p => (double) Random.Shared.NextDouble() * 1024).ToArray());

    [Test]
    public void TestFloats() => run(Enumerable.Range(0, SIZE).Select(p => (float) (Random.Shared.NextDouble() * 1024)).ToArray());

    [Test]
    public void TestDecimals() => run(Enumerable.Range(0, SIZE).Select(p => (decimal) Random.Shared.NextDouble() * 1024).ToArray());

    [Test]
    public void TestInt64s() => run(Enumerable.Range(0, SIZE).Select(p => (Int64) Random.Shared.NextInt64(Int64.MaxValue)).ToArray());

    [Test]
    public void TestUInt64s() => run(Enumerable.Range(0, SIZE).Select(p => (UInt64) Random.Shared.NextInt64(Int64.MaxValue)).ToArray());

    [Test]
    public void TestInt32s() => run(Enumerable.Range(0, SIZE).Select(p => (int) Random.Shared.Next(int.MaxValue)).ToArray());

    [Test]
    public void TestUInt32s() => run(Enumerable.Range(0, SIZE).Select(p => (uint) Random.Shared.Next(int.MaxValue)).ToArray());

    [Test]
    public void TestInt16s() => run(Enumerable.Range(0, SIZE).Select(p => (short) Random.Shared.Next(short.MaxValue)).ToArray());

    [Test]
    public void TestUInt16s() => run(Enumerable.Range(0, SIZE).Select(p => (ushort) Random.Shared.Next(ushort.MaxValue)).ToArray());

    [Test]
    public void TestBytes() => run(Enumerable.Range(0, SIZE).Select(p => (byte) Random.Shared.Next(byte.MaxValue)).ToArray());

    [Test]
    public void TestSBytes() => run(Enumerable.Range(0, SIZE).Select(p => (sbyte) Random.Shared.Next(sbyte.MaxValue)).ToArray());
}