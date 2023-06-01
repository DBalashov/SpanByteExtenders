using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class SimpleTests
{
    void run<T>(T value) where T : struct
    {
        var itemSize = Unsafe.SizeOf<T>();
        var buff = new byte[itemSize +
                            Random.Shared.Next(0, 200)]; // + some random length to buffer

        var rawDataSpan = buff.AsSpan();
        rawDataSpan.Write(value);

        var finalSpan = buff.AsSpan();
        var finalData = finalSpan.Read<T>();
        Assert.IsTrue(Equals(value, finalData));
    }

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