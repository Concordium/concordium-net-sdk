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
    public void From_when_valid_string_passed_should_create_correct_instance()
    {
        // Arrange
        var address = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";

        // Act
        var accountAddress = AccountAddress.From(address);

        // Assert
        accountAddress.AsString.Should().Be(address);
    }

    [Fact]
    public void From_when_valid_bytes_passed_should_create_correct_instance()
    {
        // Arrange
        var address = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        byte[] bytes = new Base58CheckEncoder().DecodeData(address).Skip(1).ToArray();

        // Act
        var accountAddress = AccountAddress.From(bytes);

        // Assert
        accountAddress.AsString.Should().Be(address);
    }

    [Fact]
    public void From_when_address_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var bytes = new byte[31];

        // Act
        Action result = () => AccountAddress.From(bytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("Expected length to be exactly 32 bytes");
    }

    [Fact]
    public void From_when_address_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var bytes = new byte[33];

        // Act
        Action result = () => AccountAddress.From(bytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("Expected length to be exactly 32 bytes");
    }
}
