using System;
using System.Linq;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using NBitcoin.DataEncoders;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class MemoTests
{
    [Fact]
    public void Same_Memos_AreEqual()
    {
        var memo = "message";
        var memoA = Memo.FromText(memo);
        var memoB = Memo.FromText(memo);
        Assert.Equal(memoA, memoB);
    }

    [Fact]
    public void Different_Addresses_AreNotEqual()
    {
        var memo = "message";
        var memoA = Memo.FromText(memo);
        var memoB = Memo.FromText(memo + "a");
        Assert.NotEqual(memoA, memoB);
    }

    [Theory]
    [InlineData("")]
    [InlineData("feedbeef")]
    public void FromHex_OnValidHexString_ThenGetBytes_AreEqual(string memoAsHexString)
    {
        var bytes = Convert.FromHexString(memoAsHexString);
        var memo = Memo.FromHex(memoAsHexString);
        memo.GetBytes().Skip(sizeof(UInt16)).Should().BeEquivalentTo(bytes);
    }

    [Theory]
    [InlineData("")]
    [InlineData("feedbeef")]
    public void From_OnValidHexString_ThenGetHexString_AreEqual(string memoAsHexString)
    {
        var memo = Memo.FromHex(memoAsHexString);
        memo.GetHexString().Should().BeEquivalentTo(memoAsHexString);
    }

    [Fact]
    public void FromText_OnValidString_ThenTryCBORDecode_AreEqual()
    {
        var text = "feedbEEl";
        var memo = Memo.FromText(text);
        var decoded = memo.TryCborDecodeToString();
        Assert.NotNull(decoded);
        decoded.Should().Be(text);
    }

    [Theory]
    [InlineData((Memo.MaxLength + 1) * 2)]
    [InlineData((Memo.MaxLength + 50) * 2)]
    public void FromHex_OnTooLongHexString_ThrowsException(Int16 length)
    {
        var memo = string.Concat(Enumerable.Repeat("a", length));
        Action result = () => Memo.FromHex(memo);
        result.Should().Throw<ArgumentException>();
    }

    // TODO: More exhaustive testing is needed here.
}
