using System;
using System.Linq;
using System.Buffers.Binary;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class DataTests
{
    [Fact]
    public void Same_Datas_AreEqual()
    {
        var dataA = Data.From("feedbeef");
        var dataB = Data.From("feedbeef");
        Assert.Equal(dataA, dataB);
    }

    [Fact]
    public void Different_Datas_AreNotEqual()
    {
        var dataA = Data.From("feedbeef");
        var dataB = Data.From("feedbeed");
        Assert.NotEqual(dataA, dataB);
    }

    [Fact]
    public void From_OnValidString_ToString_AreEqual()
    {
        var dataAsHexString = "feedbeef";
        var data = Data.From(dataAsHexString);
        data.ToString().Should().BeEquivalentTo(dataAsHexString);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(Data.MaxLength)]
    public void From_OnValidBytes_ReturnsCorrectValue(Int16 length)
    {
        byte[] header = new Byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(header, (UInt16)length);
        byte[] data = Enumerable.Repeat((byte)128, length).ToArray();
        byte[] bytes = header.Concat(data).ToArray();
        Data.From(data).GetBytes().Should().BeEquivalentTo(bytes);
    }

    [Theory]
    [InlineData((Data.MaxLength + 1) * 2)]
    [InlineData((Data.MaxLength + 50) * 2)]
    public void From_OnTooLongBytes_ThrowsException(Int16 length)
    {
        byte[] data = Enumerable.Repeat((byte)128, length).ToArray();
        Action result = () => Data.From(data);
        result.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(Data.MaxLength * 2 + 1)]
    [InlineData(Data.MaxLength * 2 + 100)]
    public void From_OnTooLongString_ThrowsException(Int16 length)
    {
        var data = string.Concat(Enumerable.Repeat("a", length));
        Action result = () => Data.From(data);
        result.Should().Throw<ArgumentException>();
    }
}
