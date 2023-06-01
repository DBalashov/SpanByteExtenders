namespace SpanByteExtenders.Tests;

public abstract class BaseStructTests
{
    protected TestStruct getStruct() =>
        new()
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
    
    protected bool compare(TestStruct s1, TestStruct s2) =>
        s1.sbyte1  == s2.sbyte1  && s1.byte1  == s2.byte1  && s1.short1   == s2.short1   && s1.ushort1 == s2.ushort1 &&
        s1.int1    == s2.int1    && s1.uint1  == s2.uint1  && s1.int64    == s2.int64    && s1.uint64  == s2.uint64  &&
        s1.double1 == s2.double1 && s1.float1 == s2.float1 && s1.decimal1 == s2.decimal1 && s1.guid1   == s2.guid1;
}