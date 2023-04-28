using ConcordiumNetSdk.Types;
using FluentAssertions;
using System;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class AccountNonceTests
{
    [Fact]
    public void Same_AccountNonces_AreEqual()
    {
        var nonceA = new AccountNonce(0);
        var nonceB = new AccountNonce(0);
        Assert.Equal(nonceA, nonceB);
    }

    [Fact]
    public void Different_AccountNonces_AreNotEqual()
    {
        var nonceA = new AccountNonce(0);
        var nonceB = new AccountNonce(1);
        Assert.NotEqual(nonceA, nonceB);
    }

    [Fact]
    public void GetIncrementedNonce_OnAccountNonce_ReturnsIncrementedNonce()
    {
        var nonceA = new AccountNonce(0).GetIncrementedNonce();
        var nonceB = new AccountNonce(1);
        Assert.Equal(nonceA, nonceB);
    }

    [Fact]
    public void GetIncrementedNonce_OnMaxValuedAccountNonce_ThrowsException()
    {
        var nonce = new AccountNonce(UInt64.MaxValue);
        Action result = () => nonce.GetIncrementedNonce();
        result.Should().Throw<OverflowException>();
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var nonce = new AccountNonce(100);
        var expectedSerializedNonce = new byte[] { 0, 0, 0, 0, 0, 0, 0, 100 };
        var serializedNonce = nonce.GetBytes();
        serializedNonce.Should().BeEquivalentTo(expectedSerializedNonce);
    }
}
