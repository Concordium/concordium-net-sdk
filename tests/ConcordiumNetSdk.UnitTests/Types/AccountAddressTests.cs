using System;
using System.Linq;
using System.Text;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using NBitcoin.DataEncoders;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class AccountAddressTests
{
    [Fact]
    public void From_when_valid_string_value_passed_should_create_correct_instance()
    {
        // Arrange
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";

        // Act
        var accountAddress = AccountAddress.From(addressAsBase58String);

        // Assert
        accountAddress.AsString.Should().Be(addressAsBase58String);
    }

    [Theory]
    [InlineData("3XSLuJcX")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    [InlineData("3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9Ppp")]
    public void From_when_invalid_string_value_passed_should_throw_appropriate_exception(string invalidAddressAsBase58String)
    {
        // Arrange

        // Act
        Action result = () => AccountAddress.From(invalidAddressAsBase58String);

        // Assert
        result.Should().Throw<FormatException>().WithMessage("Invalid hash of the base 58 string");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        // Arrange
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var addressAsBytes = new Base58CheckEncoder().DecodeData(addressAsBase58String).Skip(1).ToArray(); // skip version byte

        // Act
        var accountAddress = AccountAddress.From(addressAsBytes);

        // Assert
        accountAddress.AsString.Should().Be(addressAsBase58String);
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidAddressAsBytes = new byte[31];

        // Act
        Action result = () => AccountAddress.From(invalidAddressAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The account address bytes length must be 32.");
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidAddressAsBytes = new byte[33];

        // Act
        Action result = () => AccountAddress.From(invalidAddressAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The account address bytes length must be 32.");
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        // Arrange
        var expectedAddressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var address = AccountAddress.From(expectedAddressAsBase58String);

        // Act
        var addressAsBase58String = address.AsString;

        // Assert
        addressAsBase58String.Should().BeEquivalentTo(expectedAddressAsBase58String);
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        // Arrange
        var addressAsBase58String = "3XSLuJcXg6xEua6iBPnWacc3iWh93yEDMCqX8FbE3RDSbEnT9P";
        var address = AccountAddress.From(addressAsBase58String);
        var expectedAddressAsBytes = new Base58CheckEncoder().DecodeData(addressAsBase58String).Skip(1).ToArray();

        // Act
        var addressAsBytes = address.AsBytes;

        // Assert
        addressAsBytes.Should().BeEquivalentTo(expectedAddressAsBytes);
    }
}
