using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class StructTests
{
    [Test]
    public void TestMulti()
    {
        var buff = new byte[1024];

        var source = Enumerable.Range(0, 8).Select(p => new TestStruct()
                                                        {
                                                            sbyte1  = (sbyte) Random.Shared.Next(sbyte.MinValue, sbyte.MaxValue),
                                                            byte1   = (byte) Random.Shared.Next(byte.MaxValue),
                                                            short1  = (short) Random.Shared.Next(short.MinValue, short.MaxValue),
                                                            ushort1 = (ushort) Random.Shared.Next(ushort.MaxValue),
                                                            int1    = (int) Random.Shared.Next(int.MinValue, int.MaxValue),
                                                            uint1   = (uint) Random.Shared.Next(int.MaxValue),
                                                            int64   = (int) Random.Shared.Next(int.MinValue, int.MaxValue),
                                                            uint64  = (uint) Random.Shared.Next(int.MaxValue),

                                                            double1  = Random.Shared.NextDouble() * 1024,
                                                            float1   = (float) (Random.Shared.NextDouble()   * 1024),
                                                            decimal1 = (decimal) (Random.Shared.NextDouble() * 1024),
                                                            guid1    = Guid.NewGuid()
                                                        })
                               .ToArray();

        var rawDataSpan = buff.AsSpan();
        rawDataSpan.WriteStructs(source.AsSpan());

        var finalSpan = buff.AsSpan();
        var finalData = finalSpan.ReadStructs<TestStruct>(source.Length);
        Assert.IsTrue(finalData.Length == source.Length);
        for (var i = 0; i < source.Length; i++)
            Assert.IsTrue(compare(finalData[i], source[i]));
    }

    [Test]
    public void TestSingle()
    {
        var buff = new byte[1024];

        var source = new TestStruct()
                     {
                         sbyte1  = (sbyte) Random.Shared.Next(sbyte.MinValue, sbyte.MaxValue),
                         byte1   = (byte) Random.Shared.Next(byte.MaxValue),
                         short1  = (short) Random.Shared.Next(short.MinValue, short.MaxValue),
                         ushort1 = (ushort) Random.Shared.Next(ushort.MaxValue),
                         int1    = (int) Random.Shared.Next(int.MinValue, int.MaxValue),
                         uint1   = (uint) Random.Shared.Next(int.MaxValue),
                         int64   = (int) Random.Shared.Next(int.MinValue, int.MaxValue),
                         uint64  = (uint) Random.Shared.Next(int.MaxValue),

                         double1  = Random.Shared.NextDouble() * 1024,
                         float1   = (float) (Random.Shared.NextDouble()   * 1024),
                         decimal1 = (decimal) (Random.Shared.NextDouble() * 1024),
                         guid1    = Guid.NewGuid()
                     };

        var rawDataSpan = buff.AsSpan();
        rawDataSpan.WriteStruct(source);

        var finalSpan = buff.AsSpan();
        var finalData = finalSpan.ReadStruct<TestStruct>();
        Assert.IsTrue(compare(source, finalData));
    }

    bool compare(TestStruct s1, TestStruct s2) =>
        s1.sbyte1  == s2.sbyte1  && s1.byte1  == s2.byte1  && s1.short1   == s2.short1   && s1.ushort1 == s2.ushort1 &&
        s1.int1    == s2.int1    && s1.uint1  == s2.uint1  && s1.int64    == s2.int64    && s1.uint64  == s2.uint64  &&
        s1.double1 == s2.double1 && s1.float1 == s2.float1 && s1.decimal1 == s2.decimal1 && s1.guid1   == s2.guid1;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TestStruct
{
    public sbyte   sbyte1;
    public byte    byte1;
    public short   short1;
    public ushort  ushort1;
    public int     int1;
    public uint    uint1;
    public Int64   int64;
    public UInt64  uint64;
    public double  double1;
    public float   float1;
    public decimal decimal1;
    public Guid    guid1;
}