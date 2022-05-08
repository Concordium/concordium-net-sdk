using System;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.TypeTests;

public class BlockHashTests
{
    // todo: write unit tests if needed
    [Fact]
    public void Constructor_when_valid_string_passed_should_create_correct_instance()
    {
        // Arrange
        var blockHashAsBase16String = "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";

        // Act
        var blockHash = new BlockHash(blockHashAsBase16String);

        // Assert
        blockHash.AsString.Should().Be(blockHashAsBase16String);
    }
    
    [Fact]
    public void Constructor_when_valid_bytes_passed_should_create_correct_instance()
    {
        // Arrange
        var blockHashAsBase16String = "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        byte[] blockHashAsBytes = Convert.FromHexString(blockHashAsBase16String);

        // Act
        var blockHash = new BlockHash(blockHashAsBytes);

        // Assert
        blockHash.AsString.Should().Be(blockHashAsBase16String);
    }

    [Fact]
    public void Constructor_when_hash_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var blockHashAsBytes = new byte[31];

        // Act
        Action result = () => new BlockHash(blockHashAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void Constructor_when_hash_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var blockHashAsBytes = new byte[33];

        // Act
        Action result = () => new BlockHash(blockHashAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The hash bytes length must be 32.");
    }
}
