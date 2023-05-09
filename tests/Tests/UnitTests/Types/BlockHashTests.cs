using System;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.UnitTests.Types;

public class BlockHashTests
{
    [Fact]
    public void Same_BlockHashes_AreEqual()
    {
        var blockHashA = BlockHash.From(
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        var blockHashB = BlockHash.From(
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        Assert.Equal(blockHashA, blockHashB);
    }

    [Fact]
    public void Different_BlockHashes_AreNotEqual()
    {
        var blockHashA = BlockHash.From(
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        var blockHashB = BlockHash.From(
            "54c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        Assert.NotEqual(blockHashA, blockHashB);
    }

    [Fact]
    public void From_OnValidString_ToString_AreEqual()
    {
        var blockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var blockHash = BlockHash.From(blockHashAsBase16String);
        blockHash.ToString().Should().Be(blockHashAsBase16String);
    }

    [Theory]
    [InlineData("")]
    [InlineData("44c52f0dc89c")]
    [InlineData("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494affffff")]
    [InlineData("æøå")]
    public void From_OnInvalidString_ThrowsException(string invalidHashAsBase58String)
    {
        Action result = () => BlockHash.From(invalidHashAsBase58String);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_OnValidBytes_ToString_ReturnsCorrectValue()
    {
        var blockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var blockHash = BlockHash.From(blockHashAsBase16String);
        var blockHashAsBytes = blockHash.GetBytes();
        blockHash.GetBytes().Should().BeEquivalentTo(blockHashAsBytes);
    }

    [Fact]
    public void From_OnInvalidBytes_TooShort_ThrowsException()
    {
        var invalidHashAsBytes = new byte[2];
        Action result = () => BlockHash.From(invalidHashAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_OnInvalidBytes_TooLong_ThrowsException()
    {
        var invalidHashAsBytes = new byte[33];
        Action result = () => BlockHash.From(invalidHashAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var blockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var blockHash = BlockHash.From(blockHashAsBase16String);
        var expectedBlockHashAsBytes = Convert.FromHexString(blockHashAsBase16String);
        var blockHashAsBytes = blockHash.GetBytes();
        blockHashAsBytes.Should().BeEquivalentTo(expectedBlockHashAsBytes);
    }
}
