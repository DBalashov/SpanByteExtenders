using System.Runtime.InteropServices;

namespace SpanByteExtenders.Tests;

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