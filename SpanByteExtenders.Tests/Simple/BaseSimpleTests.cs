using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public abstract class BaseSimpleTests
{
    protected abstract void run<T>(T value) where T : struct;
    
    [Test]
    public void TestGuids() => run(Guid.NewGuid());

    [Test]
    public void TestDoubles() => run((double) Random.Shared.NextDouble() * 1024);

    [Test]
    public void TestFloats() => run((float) (Random.Shared.NextDouble() * 1024));

    [Test]
    public void TestDecimals() => run((decimal) Random.Shared.NextDouble() * 1024);

    [Test]
    public void TestInt64s() => run((Int64) Random.Shared.NextInt64(Int64.MaxValue));

    [Test]
    public void TestUInt64s() => run((UInt64) Random.Shared.NextInt64(Int64.MaxValue));

    [Test]
    public void TestInt32s() => run((int) Random.Shared.Next(int.MaxValue));

    [Test]
    public void TestUInt32s() => run((uint) Random.Shared.Next(int.MaxValue));

    [Test]
    public void TestInt16s() => run((short) Random.Shared.Next(short.MaxValue));

    [Test]
    public void TestUInt16s() => run((ushort) Random.Shared.Next(ushort.MaxValue));

    [Test]
    public void TestBytes() => run((byte) Random.Shared.Next(byte.MaxValue));

    [Test]
    public void TestSBytes() => run((sbyte) Random.Shared.Next(sbyte.MaxValue));
}