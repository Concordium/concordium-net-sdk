using System;
using ConcordiumNetSdk.SignKey;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.SignKey;

public class SaltTests
{
    [Fact]
    public void From_when_valid_string_value_passed_should_create_correct_instance()
    {
        // Arrange
        var saltAsBase64String = "QsY4+h31LMs974pPN6QfsA==";

        // Act
        var salt = Salt.From(saltAsBase64String);

        // Assert
        salt.AsString.Should().Be(saltAsBase64String);
    }

    [Fact]
    public void From_when_string_value_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidSaltAsBase64String = "QsY4+h31LMs97";

        // Act
        Action result = () => Salt.From(invalidSaltAsBase64String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The salt base64 encoded string length must be 24.");
    }

    [Fact]
    public void From_when_string_value_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidSaltAsBase64String = "QsY4+h31LMs974pPN6QfsA==a";

        // Act
        Action result = () => Salt.From(invalidSaltAsBase64String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The salt base64 encoded string length must be 24.");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        // Arrange
        var saltAsBase64String = "QsY4+h31LMs974pPN6QfsA==";
        var saltAsBytes = Convert.FromBase64String(saltAsBase64String);

        // Act
        var salt = Salt.From(saltAsBytes);

        // Assert
        salt.AsString.Should().Be(saltAsBase64String);
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidSaltAsBytes = new byte[15];

        // Act
        Action result = () => Salt.From(invalidSaltAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The salt bytes length must be 16.");
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidSaltAsBytes = new byte[17];

        // Act
        Action result = () => Salt.From(invalidSaltAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The salt bytes length must be 16.");
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        // Arrange
        var expectedSaltAsBase64String = "QsY4+h31LMs974pPN6QfsA==";
        var salt = Salt.From(expectedSaltAsBase64String);

        // Act
        var saltAsBase64String = salt.AsString;

        // Assert
        saltAsBase64String.Should().BeEquivalentTo(expectedSaltAsBase64String);
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        // Arrange
        var saltAsBase64String = "QsY4+h31LMs974pPN6QfsA==";
        var salt = Salt.From(saltAsBase64String);
        var expectedSaltAsBytes = Convert.FromBase64String(saltAsBase64String);

        // Act
        var saltAsBytes = salt.AsBytes;

        // Assert
        saltAsBytes.Should().BeEquivalentTo(expectedSaltAsBytes);
    }
}
