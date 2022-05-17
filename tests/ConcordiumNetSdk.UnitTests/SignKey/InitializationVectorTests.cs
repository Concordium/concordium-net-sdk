using System;
using ConcordiumNetSdk.SignKey;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.SignKey;

public class InitializationVectorTests
{
    [Fact]
    public void From_when_valid_string_value_passed_should_create_correct_instance()
    {
        // Arrange
        var initializationVectorAsBase64String = "kzyQ24xum3WibCKfvngMlg==";

        // Act
        var initializationVector = InitializationVector.From(initializationVectorAsBase64String);

        // Assert
        initializationVector.AsString.Should().Be(initializationVectorAsBase64String);
    }

    [Fact]
    public void From_when_string_value_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidInitializationVectorAsBase64String = "kzyQ24xum3WibCKfvn";

        // Act
        Action result = () => InitializationVector.From(invalidInitializationVectorAsBase64String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The initialization vector base64 encoded string length must be 24.");
    }

    [Fact]
    public void From_when_string_value_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidInitializationVectorAsBase64String = "kzyQ24xum3WibCKfvngMlg==a";

        // Act
        Action result = () => InitializationVector.From(invalidInitializationVectorAsBase64String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The initialization vector base64 encoded string length must be 24.");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        // Arrange
        var initializationVectorAsBase64String = "kzyQ24xum3WibCKfvngMlg==";
        var initializationVectorAsBytes = Convert.FromBase64String(initializationVectorAsBase64String);

        // Act
        var initializationVector = InitializationVector.From(initializationVectorAsBytes);

        // Assert
        initializationVector.AsString.Should().Be(initializationVectorAsBase64String);
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidInitializationVectorAsBytes = new byte[15];

        // Act
        Action result = () => InitializationVector.From(invalidInitializationVectorAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The initialization vector bytes length must be 16.");
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidInitializationVectorAsBytes = new byte[17];

        // Act
        Action result = () => InitializationVector.From(invalidInitializationVectorAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The initialization vector bytes length must be 16.");
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        // Arrange
        var expectedInitializationVectorAsBase64String = "kzyQ24xum3WibCKfvngMlg==";
        var initializationVector = InitializationVector.From(expectedInitializationVectorAsBase64String);

        // Act
        var initializationVectorAsBase64String = initializationVector.AsString;

        // Assert
        initializationVectorAsBase64String.Should().BeEquivalentTo(expectedInitializationVectorAsBase64String);
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        // Arrange
        var initializationVectorAsBase64String = "kzyQ24xum3WibCKfvngMlg==";
        var initializationVector = InitializationVector.From(initializationVectorAsBase64String);
        var expectedInitializationVectorAsBytes = Convert.FromBase64String(initializationVectorAsBase64String);

        // Act
        var initializationVectorAsBytes = initializationVector.AsBytes;

        // Assert
        initializationVectorAsBytes.Should().BeEquivalentTo(expectedInitializationVectorAsBytes);
    }
}
