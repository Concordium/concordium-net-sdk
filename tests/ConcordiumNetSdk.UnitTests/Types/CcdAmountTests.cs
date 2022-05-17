using System;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class CcdAmountTests
{
    [Fact]
    public void Zero_should_create_correct_instance()
    {
        // Arrange

        // Act
        var ccdAmount = CcdAmount.Zero;

        // Assert
        ccdAmount.MicroCcdValue.Should().Be(0);
    }

    [Fact]
    public void FromMicroCcd_when_valid_ulong_value_passed_should_create_correct_instance()
    {
        // Arrange
        var value = ulong.MaxValue;

        // Act
        var ccdAmount = CcdAmount.FromMicroCcd(value);

        // Assert
        ccdAmount.MicroCcdValue.Should().Be(value);
    }

    [Fact]
    public void FromMicroCcd_when_valid_int_value_passed_should_create_correct_instance()
    {
        // Arrange
        var value = int.MaxValue;

        // Act
        var ccdAmount = CcdAmount.FromMicroCcd(value);

        // Assert
        ccdAmount.MicroCcdValue.Should().Be((ulong) value);
    }

    [Fact]
    public void FromMicroCcd_when_negative_int_value_passed_should_throw_appropriate_exception()
    {
        // Arrange
        var value = int.MinValue;

        // Act
        Action result = () => CcdAmount.FromMicroCcd(value);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("Cannot represent negative numbers. (Parameter 'microCcd')");
    }

    [Fact]
    public void FromCcd_when_valid_value_passed_should_create_correct_instance()
    {
        // Arrange
        var value = int.MaxValue;

        // Act
        var ccdAmount = CcdAmount.FromCcd(value);

        // Assert
        ccdAmount.MicroCcdValue.Should().Be((ulong) value * 1_000_000);
    }

    [Fact]
    public void FromCcd_when_negative_value_passed_should_throw_appropriate_exception()
    {
        // Arrange
        var value = int.MinValue;

        // Act
        Action result = () => CcdAmount.FromCcd(value);

        // Assert
        result.Should().Throw<ArgumentException>().WithMessage("Cannot represent negative numbers. (Parameter 'ccd')");
    }

    [Fact]
    public void SerializeToBytes_should_return_bytes_in_UInt64_Big_Endian_format()
    {
        // Arrange
        var ccdAmount = CcdAmount.FromMicroCcd(100);
        var expectedSerializedCcdAmount = new byte[] {0, 0, 0, 0, 0, 0, 0, 100};

        // Act
        var serializedCcdAmount = ccdAmount.SerializeToBytes();

        // Assert
        serializedCcdAmount.Should().BeEquivalentTo(expectedSerializedCcdAmount);
    }
}
