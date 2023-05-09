using System;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.UnitTests.Types;

public class AccountNonceTests
{
    [Fact]
    public void Same_AccountNonces_AreEqual()
    {
        var nonceA = AccountSequenceNumber.From(1);
        var nonceB = AccountSequenceNumber.From(1);
        Assert.Equal(nonceA, nonceB);
    }

    [Fact]
    public void Different_AccountNonces_AreNotEqual()
    {
        var nonceA = AccountSequenceNumber.From(1);
        var nonceB = AccountSequenceNumber.From(2);
        Assert.NotEqual(nonceA, nonceB);
    }

    [Fact]
    public void GetIncrementedNonce_OnAccountNonce_ReturnsIncrementedNonce()
    {
        var nonceA = AccountSequenceNumber.From(1).GetIncrementedNonce();
        var nonceB = AccountSequenceNumber.From(2);
        Assert.Equal(nonceA, nonceB);
    }

    [Fact]
    public void From_Zero_ThrowsException()
    {
        Action result = () => AccountSequenceNumber.From(0);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetIncrementedNonce_OnMaxValuedAccountNonce_ThrowsException()
    {
        var nonce = AccountSequenceNumber.From(ulong.MaxValue);
        Action result = () => nonce.GetIncrementedNonce();
        result.Should().Throw<OverflowException>();
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var nonce = AccountSequenceNumber.From(100);
        var expectedSerializedNonce = new byte[] { 0, 0, 0, 0, 0, 0, 0, 100 };
        var serializedNonce = nonce.GetBytes();
        serializedNonce.Should().BeEquivalentTo(expectedSerializedNonce);
    }
}
