using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class MintRateTests
{
    [Theory]
    [InlineData(2506,0)]
    [InlineData(256,0)]
    [InlineData(256,5)]
    [InlineData(2,5)]
    public void GivenDecimal_WhenCalculateMintRate_ThenCorrectMantissaAndExponent(
        uint mantissa, uint exponent)
    {
        // Arrange
        var mintRateGrpc = new Grpc.V2.MintRate
        {
            Mantissa = mantissa,
            Exponent = exponent
        };
        var mintRate = Concordium.Sdk.Types.MintRate.From(mintRateGrpc);
        var calculated = mintRate.GetDecimal();

        // Act
        var mintRateActual = Concordium.Sdk.Types.MintRate.From(calculated);

        // Assert
        var (actual_exponent, actual_mantissa) = mintRateActual.GetValues();
        actual_mantissa.Should().Be(mantissa);
        actual_exponent.Should().Be(exponent);
    }
}
