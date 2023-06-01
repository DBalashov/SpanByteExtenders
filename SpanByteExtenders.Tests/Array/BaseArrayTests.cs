using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public abstract class BaseArrayTests
{
    const int SIZE = 16;

    protected abstract void run<T>(T[] rawData) where T : struct;
    
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