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
        var transactionHashAsBase16String =
            "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        var transactionHash = TransactionHash.From(transactionHashAsBase16String);
        transactionHash.ToString().Should().Be(transactionHashAsBase16String);
    }

    [Fact]
    public void From_when_string_value_is_too_short_should_throw_appropriate_exception()
    {
        var invalidTransactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c";
        Action result = () => TransactionHash.From(invalidTransactionHashAsBase16String);
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The hash base16 encoded string length must be 64.");
    }

    [Fact]
    public void From_when_string_value_is_too_long_should_throw_appropriate_exception()
    {
        var invalidTransactionHashAsBase16String =
            "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37ff";
        Action result = () => TransactionHash.From(invalidTransactionHashAsBase16String);
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The hash base16 encoded string length must be 64.");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        var transactionHashAsBase16String =
            "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        var transactionHashAsBytes = Convert.FromHexString(transactionHashAsBase16String);
        var transactionHash = TransactionHash.From(transactionHashAsBytes);
        transactionHash.ToString().Should().Be(transactionHashAsBase16String);
    }

    [Fact]
    public void From_when_bytes_value_is_too_short_should_throw_appropriate_exception()
    {
        var invalidTransactionHashAsBytes = new byte[31];
        Action result = () => TransactionHash.From(invalidTransactionHashAsBytes);
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void From_when_bytes_value_is_too_long_should_throw_appropriate_exception()
    {
        var invalidTransactionHashAsBytes = new byte[33];
        Action result = () => TransactionHash.From(invalidTransactionHashAsBytes);
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        var transactionHashAsBase16String =
            "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        var transactionHash = TransactionHash.From(transactionHashAsBase16String);
        var expectedTransactionHashAsBytes = Convert.FromHexString(transactionHashAsBase16String);
        var transactionHashAsBytes = transactionHash.GetBytes();
        transactionHashAsBytes.Should().BeEquivalentTo(expectedTransactionHashAsBytes);
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        // Arrange
        var expectedTransactionHashAsBase16String =
            "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        var transactionHash = TransactionHash.From(expectedTransactionHashAsBase16String);

        // Act
        var transactionHashAsBase16String = transactionHash.GetBytes();

        // Assert
        transactionHashAsBase16String
            .Should()
            .BeEquivalentTo(expectedTransactionHashAsBase16String);
    }
}
