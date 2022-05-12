using System;
using ConcordiumNetSdk.SignKey;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.SignKey;

public class CipherTextTests
{
    [Fact]
    public void From_when_valid_string_value_passed_should_create_correct_instance()
    {
        // Arrange
        var cipherTextAsBase64String = "9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE=";

        // Act
        var cipherText = CipherText.From(cipherTextAsBase64String);

        // Assert
        cipherText.AsString.Should().Be(cipherTextAsBase64String);
    }

    [Fact]
    public void From_when_string_value_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidCipherTextAsBase64String = "9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6";

        // Act
        Action result = () => CipherText.From(invalidCipherTextAsBase64String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The cipher text base64 encoded string length must be 108.");
    }

    [Fact]
    public void From_when_string_value_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidCipherTextAsBase64String = "9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE=a";

        // Act
        Action result = () => CipherText.From(invalidCipherTextAsBase64String);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The cipher text base64 encoded string length must be 108.");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        // Arrange
        var cipherTextAsBase64String = "9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE=";
        var cipherTextAsBytes = Convert.FromBase64String(cipherTextAsBase64String);

        // Act
        var cipherText = CipherText.From(cipherTextAsBytes);

        // Assert
        cipherText.AsString.Should().Be(cipherTextAsBase64String);
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_short_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidCipherTextAsBytes = new byte[79];

        // Act
        Action result = () => CipherText.From(invalidCipherTextAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The cipher text bytes length must be 80.");
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_long_should_throw_appropriate_exception()
    {
        // Arrange
        var invalidCipherTextAsBytes = new byte[81];

        // Act
        Action result = () => CipherText.From(invalidCipherTextAsBytes);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("The cipher text bytes length must be 80.");
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        // Arrange
        var expectedCipherTextAsBase64String = "9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE=";
        var cipherText = CipherText.From(expectedCipherTextAsBase64String);

        // Act
        var cipherTextAsBase64String = cipherText.AsString;

        // Assert
        cipherTextAsBase64String.Should().BeEquivalentTo(expectedCipherTextAsBase64String);
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        // Arrange
        var cipherTextAsBase64String = "9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE=";
        var cipherText = CipherText.From(cipherTextAsBase64String);
        var expectedCipherTextAsBytes = Convert.FromBase64String(cipherTextAsBase64String);

        // Act
        var cipherTextAsBytes = cipherText.AsBytes;

        // Assert
        cipherTextAsBytes.Should().BeEquivalentTo(expectedCipherTextAsBytes);
    }
}
