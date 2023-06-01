using NUnit.Framework;

namespace SpanByteExtenders.Tests;

public class SpanStructTests : BaseStructTests
{
    [Test]
    public void TestMulti()
    {
        var buff = new byte[1024];

        var source = Enumerable.Range(0, 8).Select(p => getStruct()).ToArray();

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

        var source = getStruct();

        var rawDataSpan = buff.AsSpan();
        rawDataSpan.WriteStruct(source);

        var finalSpan = buff.AsSpan();
        var finalData = finalSpan.ReadStruct<TestStruct>();
        Assert.IsTrue(compare(source, finalData));
    }
}