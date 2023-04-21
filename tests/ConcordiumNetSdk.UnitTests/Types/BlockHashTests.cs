using System;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class BlockHashTests
{
    [Fact]
    public void From_when_valid_string_value_passed_should_create_correct_instance()
    {
        // Arrange
        var blockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";

        // Act
        var blockHash = BlockHash.From(blockHashAsBase16String);

        // Assert
        blockHash.ToString().Should().Be(blockHashAsBase16String);
    }

    [Fact]
    public void From_when_string_value_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidBlockHashAsBase16String = "44c52f0dc89c5244b494223c96f037b5e312572";

        // Act
        Action result = () => BlockHash.From(invalidBlockHashAsBase16String);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The hash base16 encoded string length must be 64.");
    }

    [Fact]
    public void From_when_string_value_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidBlockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494afff";

        // Act
        Action result = () => BlockHash.From(invalidBlockHashAsBase16String);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The hash base16 encoded string length must be 64.");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        // Arrange
        var blockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var blockHashAsBytes = Convert.FromHexString(blockHashAsBase16String);

        // Act
        var blockHash = BlockHash.From(blockHashAsBytes);

        // Assert
        blockHash.AsString.Should().Be(blockHashAsBase16String);
    }

    [Fact]
    public void From_when_bytes_value_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidBlockHashAsBytes = new byte[31];

        // Act
        Action result = () => BlockHash.From(invalidBlockHashAsBytes);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void From_when_bytes_value_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidBlockHashAsBytes = new byte[33];

        // Act
        Action result = () => BlockHash.From(invalidBlockHashAsBytes);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The hash bytes length must be 32.");
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        // Arrange
        var expectedBlockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var blockHash = BlockHash.From(expectedBlockHashAsBase16String);

        // Act
        var blockHashAsBase16String = blockHash.ToString();

        // Assert
        blockHashAsBase16String.Should().BeEquivalentTo(expectedBlockHashAsBase16String);
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        // Arrange
        var blockHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var blockHash = BlockHash.From(blockHashAsBase16String);
        var expectedBlockHashAsBytes = Convert.FromHexString(blockHashAsBase16String);

        // Act
        var blockHashAsBytes = blockHash.GetBytes();

        // Assert
        blockHashAsBytes.Should().BeEquivalentTo(expectedBlockHashAsBytes);
    }
}
