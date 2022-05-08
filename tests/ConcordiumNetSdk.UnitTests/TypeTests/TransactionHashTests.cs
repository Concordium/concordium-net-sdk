using System;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.TypeTests;

public class TransactionHashTests
{
    // todo: write unit tests if needed
    [Fact]
    public void Constructor_when_valid_string_passed_should_create_correct_instance()
    {
        // Arrange
        var transactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";

        // Act
        var transactionHash = new TransactionHash(transactionHashAsBase16String);

        // Assert
        transactionHash.AsString.Should().Be(transactionHashAsBase16String);
    }
    
    [Fact]
    public void Constructor_when_valid_bytes_passed_should_create_correct_instance()
    {
        // Arrange
        var transactionHashAsBase16String = "b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37";
        byte[] transactionHashAsBytes = Convert.FromHexString(transactionHashAsBase16String);

        // Act
        var transactionHash = new TransactionHash(transactionHashAsBytes);

        // Assert
        transactionHash.AsString.Should().Be(transactionHashAsBase16String);
    }

    [Fact]
    public void Constructor_when_hash_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var transactionHashAsBytes = new byte[31];

        // Act
        Action result = () => new TransactionHash(transactionHashAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void Constructor_when_hash_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var transactionHashAsBytes = new byte[33];

        // Act
        Action result = () => new TransactionHash(transactionHashAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }
}
