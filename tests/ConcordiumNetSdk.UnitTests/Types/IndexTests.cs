using System;
using FluentAssertions;
using Xunit;
using Index = ConcordiumNetSdk.Types.Index;

namespace ConcordiumNetSdk.UnitTests.Types;

public class IndexTests
{
    // todo: write unit tests if needed
    [Fact]
    public void Create_when_valid_value_passed_should_create_correct_instance()
    {
        // Arrange
        var value = 255;

        // Act
        var index = Index.Create(value);

        // Assert
        index.Value.Should().Be((byte) value);
    }

    [Fact]
    public void Create_when_too_large_value_passed_should_throw_appropriate_exception()
    {
        // Arrange
        var value = 256;

        // Act
        Action result = () => Index.Create(value);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage($"Key index cannot exceed the max value '255'. Passed '{value}'.");
    }

    [Fact]
    public void Create_when_negative_value_passed_should_throw_appropriate_exception()
    {
        // Arrange
        var value = -1;

        // Act
        Action result = () => Index.Create(value);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("Key index cannot be negative.");
    }

    [Fact]
    public void SerializeToBytes_should_return_correct_data()
    {
        // Arrange
        var index = Index.Create(100);
        var expectedSerializedIndex = new byte[] {100};

        // Act
        var serializedIndex = index.SerializeToBytes();

        // Assert
        serializedIndex.Should().BeEquivalentTo(expectedSerializedIndex);
    }
}
