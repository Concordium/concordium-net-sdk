using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class MintRateTests
{
    [Theory]
    [InlineData(2506, 0)]
    [InlineData(256, 0)]
    [InlineData(256, 5)]
    [InlineData(2, 5)]
    public void GivenDecimal_WhenCalculateMintRate_ThenCorrectMantissaAndExponent(
        uint mantissa, uint exponent)
    {
        // Arrange
        var mintRateGrpc = new Grpc.V2.MintRate
        {
            Mantissa = mantissa,
            Exponent = exponent
        };
        var mintRate = MintRate.From(mintRateGrpc);
        var calculated = mintRate.AsDecimal();

        // Act
        var mintRateActual = MintRate.From(calculated);

        // Assert
        var (actual_exponent, actual_mantissa) = mintRateActual.GetValues();
        actual_mantissa.Should().Be(mantissa);
        actual_exponent.Should().Be(exponent);
    }


    /// <summary>
    /// This validates exponent above 255 (which would never be returned) would resolve in decimal of value zero
    /// when calling <see cref="MintRate.From"/>
    /// </summary>
    [Fact]
    public void GivenExponentAbove255_WhenGettingDecimal_ThenDecimalZero()
    {
        // Arrange
        const decimal expected = 0m;
        var mintRateGrpc = new Grpc.V2.MintRate
        {
            Mantissa = 1,
            Exponent = 256
        };
        var mintRate = MintRate.From(mintRateGrpc);

        // Act
        var actual = mintRate.AsDecimal();

        // Assert
        actual.Should().Be(expected);
    }
}
