using System;
using System.Linq;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using NBitcoin.DataEncoders;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.TypeTests;

public class AccountAddressTests
{
    // todo: write more unit tests if needed

    [Fact]
    public void Constructor_when_valid_string_passed_should_create_correct_instance()
    {
        // Arrange
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";

        // Act
        var accountAddress = new AccountAddress(addressAsBase58String);

        // Assert
        accountAddress.AsString.Should().Be(addressAsBase58String);
    }

    [Fact]
    public void Constructor_when_valid_bytes_passed_should_create_correct_instance()
    {
        // Arrange
        var address = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        byte[] addressAsBytes = new Base58CheckEncoder().DecodeData(address).Skip(1).ToArray();

        // Act
        var accountAddress = new AccountAddress(addressAsBytes);

        // Assert
        accountAddress.AsString.Should().Be(address);
    }

    [Fact]
    public void Constructor_when_address_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var addressAsBytes = new byte[31];

        // Act
        Action result = () => new AccountAddress(addressAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The address bytes length must be 32.");
    }

    [Fact]
    public void Constructor_when_address_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var addressAsBytes = new byte[33];

        // Act
        Action result = () => new AccountAddress(addressAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The address bytes length must be 32.");
    }
}
