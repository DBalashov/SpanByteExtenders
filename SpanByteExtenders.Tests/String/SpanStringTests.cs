using System.Text;
using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class SpanStringTests
{
    [Test]
    public void TestSingle()
    {
        var buff = new byte[1024];
        var s1   = Path.GetTempPath();

        var span1 = buff.AsSpan();
        span1.WriteString(s1);

        var span2 = buff.AsSpan();
        var s2    = span2.ReadString(Encoding.UTF8.GetByteCount(s1));
        Assert.IsTrue(string.Compare(s1, s2, StringComparison.Ordinal) == 0);
    }

    [Test]
    public void TestPrefixed()
    {
        foreach (var rsp in new[] {ReadStringPrefix.Byte, ReadStringPrefix.Short, ReadStringPrefix.Int})
        {
            var buff = new byte[1024];
            var s1   = Path.GetTempPath();

            var span1 = buff.AsSpan();
            span1.WritePrefixedString(s1, rsp);

            var span2 = buff.AsSpan();
            var s2    = span2.ReadPrefixedString(rsp);
            Assert.IsTrue(string.Compare(s1, s2, StringComparison.Ordinal) == 0);
        }
    }
}