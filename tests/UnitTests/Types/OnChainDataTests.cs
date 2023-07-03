using System;
using System.Buffers.Binary;
using System.Linq;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class OnChainDataTests
{
    [Fact]
    public void Same_Datas_HaveSameHashCode()
    {
        var dataA = OnChainData.FromHex("feedbeef");
        var dataB = OnChainData.FromHex("feedbeef");
        Assert.Equal(dataA.GetHashCode(), dataB.GetHashCode());
    }

    [Fact]
    public void Same_Datas_AreEqual()
    {
        var dataA = OnChainData.FromHex("feedbeef");
        var dataB = OnChainData.FromHex("feedbeef");
        Assert.Equal(dataA, dataB);
    }

    [Fact]
    public void Different_Datas_AreNotEqual()
    {
        var dataA = OnChainData.FromHex("feedbeef");
        var dataB = OnChainData.FromHex("feedbeed");
        Assert.NotEqual(dataA, dataB);
    }

    [Fact]
    public void FromHex_OnValidHexString_ToString_AreEqual()
    {
        var dataAsHexString = "feedbeef";
        var data = OnChainData.FromHex(dataAsHexString);
        data.ToString().Should().BeEquivalentTo(dataAsHexString);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(OnChainData.MaxLength)]
    public void From_OnValidBytes_ReturnsCorrectValue(short length)
    {
        var header = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(header, (ushort)length);
        var data = Enumerable.Repeat((byte)128, length).ToArray();
        var bytes = header.Concat(data).ToArray();
        OnChainData.From(data).ToBytes().Should().BeEquivalentTo(bytes);
    }

    [Theory]
    [InlineData(OnChainData.MaxLength + 1)]
    [InlineData(OnChainData.MaxLength + 50)]
    public void From_OnTooManyBytes_ThrowsException(short length)
    {
        var data = Enumerable.Repeat((byte)128, length).ToArray();
        Action result = () => OnChainData.From(data);
        result.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData((OnChainData.MaxLength + 1) * 2)]
    [InlineData((OnChainData.MaxLength + 100) * 2)]
    public void FromHex_OnTooLongHexString_ThrowsException(short length)
    {
        var data = string.Concat(Enumerable.Repeat("a", length));
        Action result = () => OnChainData.FromHex(data);
        result.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(((OnChainData.MaxLength - 1) * 2) + 1)]
    public void FromHex_OnWrongParityHexString_ThrowsException(short length)
    {
        var data = string.Concat(Enumerable.Repeat("a", length));
        Action result = () => OnChainData.FromHex(data);
        result.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(OnChainData.MaxLength - 2)]
    public void FromTextEncodeAsCBOR_ValidText_TryToCBOR_ReturnsCorrectValue(short length)
    {
        var data = string.Concat(Enumerable.Repeat("a", length));
        OnChainData
            .FromTextEncodeAsCBOR(data)
            .TryCborDecodeToString()
            .Should()
            .BeEquivalentTo(data);
    }

    [Theory]
    [InlineData(OnChainData.MaxLength - 1)]
    [InlineData(OnChainData.MaxLength + 100)]
    public void FromTextEncodeAsCBOR_TooLongText_ThrowsException(short length)
    {
        var data = string.Concat(Enumerable.Repeat("a", length));
        Action result = () => OnChainData.FromTextEncodeAsCBOR(data);
        result.Should().Throw<ArgumentException>();
    }
}
