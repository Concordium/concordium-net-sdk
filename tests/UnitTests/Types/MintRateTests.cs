using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class MintRateTests
{
    [Theory]
    [InlineData(255, true)]
    [InlineData(256, false)]
    public void WhenParseMintRate_ThenReturnCorrectly(uint exponent, bool expected)
    {
        // Arrange
        const uint actualMantissa = 42u;

        // Act
        var succeeded = MintRate.TryParse(exponent, actualMantissa, out var mintRate);

        // Assert
        succeeded.Should().Be(expected);
        if (expected)
        {
            mintRate.Should().NotBeNull();
            var (actual, mantissa) = mintRate!.Value.GetValues();
            actual.Should().Be(exponent);
            mantissa.Should().Be(actualMantissa);
        }
        else
        {
            mintRate.Should().BeNull();
        }
    }
}
