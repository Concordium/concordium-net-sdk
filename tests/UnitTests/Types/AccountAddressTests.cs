using System;
using System.Linq;
using FluentAssertions;
using NBitcoin.DataEncoders;
using Xunit;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class AccountAddressTests
{
    [Fact]
    public void Same_Addresses_HaveSameHashCode()
    {

        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var accountAddressA = AccountAddress.From(addressAsBase58String);
        var accountAddressB = AccountAddress.From(addressAsBase58String);
        Assert.Equal(accountAddressA.GetHashCode(), accountAddressB.GetHashCode());
    }

    [Fact]
    public void Same_Addresses_AreEqual()
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var accountAddressA = AccountAddress.From(addressAsBase58String);
        var accountAddressB = AccountAddress.From(addressAsBase58String);
        Assert.Equal(accountAddressA, accountAddressB);
    }

    [Fact]
    public void GivenEqualOperator_WhenUsingSameAddress_ThenEquals()
    {
        // Arrange
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var accountAddressA = AccountAddress.From(addressAsBase58String);
        var accountAddressB = AccountAddress.From(addressAsBase58String);

        // Act
        var equals = accountAddressA == accountAddressB;

        // Assert
        Assert.True(equals);
    }

    [Fact]
    public void Different_Addresses_AreNotEqual()
    {
        var addressAsBase58StringA = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var addressAsBase58StringB = "3QuZ47NkUk5icdDSvnfX8HiJzCnSRjzi6KwGEmqgQ7hCXNBTWN";
        var accountAddressA = AccountAddress.From(addressAsBase58StringA);
        var accountAddressB = AccountAddress.From(addressAsBase58StringB);
        Assert.NotEqual(accountAddressA, accountAddressB);
    }

    [Fact]
    public void From_OnValidString_ToString_AreEqual()
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var accountAddress = AccountAddress.From(addressAsBase58String);
        accountAddress.ToString().Should().Be(addressAsBase58String);
    }

    [Theory]
    [InlineData("")]
    [InlineData("3XSLuJcX")]
    [InlineData("3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9Pppppppp")]
    [InlineData("æøå")]
    public void From_OnInvalidString_ThrowsException(string invalidAddressAsBase58String)
    {
        Action result = () => AccountAddress.From(invalidAddressAsBase58String);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_OnValidBytes_ToString_ReturnsCorrectValue()
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var addressAsBytes = new Base58CheckEncoder()
            .DecodeData(addressAsBase58String)
            .Skip(1) // Remove version byte.
            .ToArray();
        var accountAddress = AccountAddress.From(addressAsBytes);
        accountAddress.ToString().Should().Be(addressAsBase58String);
    }

    [Fact]
    public void From_OnInvalidBytes_TooShort_ThrowsException()
    {
        var invalidAddressAsBytes = new byte[31];
        Action result = () => AccountAddress.From(invalidAddressAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_OnInvalidBytes_TooLong_ThrowsException()
    {
        var invalidAddressAsBytes = new byte[33];
        Action result = () => AccountAddress.From(invalidAddressAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToBytes_ReturnsCorrectValue()
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var address = AccountAddress.From(addressAsBase58String);
        var expectedAddressAsBytes = new Base58CheckEncoder()
            .DecodeData(addressAsBase58String)
            .Skip(1) // Remove version byte.
            .ToArray();
        var addressAsBytes = address.ToBytes();
        addressAsBytes.Should().Equal(expectedAddressAsBytes);
    }

    [Theory]
    [InlineData(0, new byte[] { 0, 0, 0 })]
    [InlineData(1, new byte[] { 0, 0, 1 })]
    [InlineData((1 << 24) - 1, new byte[] { 255, 255, 255 })]
    public void GetNthAlias_OnValidAlias_ReturnsCorrectValue(uint alias, byte[] aliasByteValue)
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var address = AccountAddress.From(addressAsBase58String);
        var firstAliasBytes = address.ToBytes().Take(29).Concat(aliasByteValue).ToArray();
        address.GetNthAlias(alias).ToBytes().Should().Equal(firstAliasBytes);
    }

    [Theory]
    [InlineData(1 << 24)]
    public void GetNthAlias_OnOutOfRangeAlias_ThrowsException(uint alias)
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var address = AccountAddress.From(addressAsBase58String);
        Action result = () => address.GetNthAlias(alias);
        result.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void IsAliasOf_OnDifferentAddresses_WithSameAliasValue_ReturnsFalse()
    {
        var addressAsBase58StringA = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var addressAsBase58StringB = "3QuZ47NkUk5icdDSvnfX8HiJzCnSRjzi6KwGEmqgQ7hCXNBTWN";
        var aliasA = AccountAddress.From(addressAsBase58StringA).GetNthAlias(0);
        var aliasB = AccountAddress.From(addressAsBase58StringB).GetNthAlias(0);
        Assert.False(aliasA.IsAliasOf(aliasB));
    }

    [Fact]
    public void IsAliasOf_OnSameAddress_WithDifferentAliasValue_ReturnsTrue()
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var address = AccountAddress.From(addressAsBase58String);
        var aliasA = address.GetNthAlias(0);
        var aliasB = address.GetNthAlias(1);
        Assert.True(aliasA.IsAliasOf(aliasB));
    }

    [Fact]
    public void IsAliasOf_OnSameAddress_WithSameAliasValue_ReturnsTrue()
    {
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var address = AccountAddress.From(addressAsBase58String);
        var aliasA = address.GetNthAlias(0);
        var aliasB = address.GetNthAlias(0);
        Assert.True(aliasA.IsAliasOf(aliasB));
    }
}
