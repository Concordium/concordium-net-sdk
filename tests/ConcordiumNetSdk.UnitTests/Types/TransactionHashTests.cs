using System;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class TransactionHashTests
{
    [Fact]
    public void From_when_valid_string_value_passed_should_create_correct_instance()
    {
        // Arrange
        var transactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";

        // Act
        var transactionHash = TransactionHash.From(transactionHashAsBase16String);

        // Assert
        transactionHash.AsString.Should().Be(transactionHashAsBase16String);
    }

    [Fact]
    public void From_when_string_value_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidTransactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c";

        // Act
        Action result = () => TransactionHash.From(invalidTransactionHashAsBase16String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash base16 encoded string length must be 64.");
    }

    [Fact]
    public void From_when_string_value_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidTransactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37ff";

        // Act
        Action result = () => TransactionHash.From(invalidTransactionHashAsBase16String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash base16 encoded string length must be 64.");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        // Arrange
        var transactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        var transactionHashAsBytes = Convert.FromHexString(transactionHashAsBase16String);

        // Act
        var transactionHash = TransactionHash.From(transactionHashAsBytes);

        // Assert
        transactionHash.AsString.Should().Be(transactionHashAsBase16String);
    }

    [Fact]
    public void From_when_bytes_value_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidTransactionHashAsBytes = new byte[31];

        // Act
        Action result = () => TransactionHash.From(invalidTransactionHashAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void From_when_bytes_value_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidTransactionHashAsBytes = new byte[33];

        // Act
        Action result = () => TransactionHash.From(invalidTransactionHashAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        // Arrange
        var transactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        var transactionHash = TransactionHash.From(transactionHashAsBase16String);
        var expectedTransactionHashAsBytes = Convert.FromHexString(transactionHashAsBase16String);

        // Act
        var transactionHashAsBytes = transactionHash.AsBytes;

        // Assert
        transactionHashAsBytes.Should().BeEquivalentTo(expectedTransactionHashAsBytes);
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        // Arrange
        var expectedTransactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        var transactionHash = TransactionHash.From(expectedTransactionHashAsBase16String);

        // Act
        var transactionHashAsBase16String = transactionHash.AsString;

        // Assert
        transactionHashAsBase16String.Should().BeEquivalentTo(expectedTransactionHashAsBase16String);
    }
}
