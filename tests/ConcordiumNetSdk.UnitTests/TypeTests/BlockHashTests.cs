using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.TypeTests;

public class BlockHashTests
{
    // todo: write unit tests if needed
    [Fact]
    public void From_when_valid_string_passed_should_create_correct_instance()
    {
        // Arrange
        var blockHashString = "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";

        // Act
        var blockHash = BlockHash.From(blockHashString);

        // Assert
        blockHash.AsString.Should().Be(blockHashString);
    }
}