using System;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class CcdAmountTests
{
    [Fact]
    public void GivenEqualOperator_WhenUsingSameAddress_ThenEquals()
    {
        // Arrange
        var ccdAmountA = CcdAmount.FromCcd(1);
        var ccdAmountB = CcdAmount.FromCcd(1);

        // Act
        var equals = ccdAmountA == ccdAmountB;

        // Assert
        equals.Should().BeTrue();
    }

    [Fact]
    public void WhenSame_CcdAmounts_AreEqual()
    {
        var ccdAmountA = CcdAmount.FromCcd(1);
        var ccdAmountB = CcdAmount.FromCcd(1);
        Assert.Equal(ccdAmountA.GetHashCode(), ccdAmountB.GetHashCode());
    }

    [Fact]
    public void Same_CcdAmounts_AreEqual()
    {
        var ccdAmountA = CcdAmount.FromCcd(1);
        var ccdAmountB = CcdAmount.FromCcd(1);
        Assert.Equal(ccdAmountA.GetHashCode(), ccdAmountB.GetHashCode());
    }

    [Fact]
    public void Different_CcdAmounts_AreNotEqual()
    {
        var ccdAmountA = CcdAmount.FromCcd(1);
        var ccdAmountB = CcdAmount.FromCcd(2);
        Assert.NotEqual(ccdAmountA, ccdAmountB);
    }

    [Fact]
    public void Same_CcdAmounts_HaveSameHashCode()
    {
        var ccdAmountA = CcdAmount.FromCcd(1);
        var ccdAmountB = CcdAmount.FromCcd(1);
        Assert.Equal(ccdAmountA, ccdAmountB);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(ulong.MaxValue / CcdAmount.MicroCcdPerCcd)]
    public void FromCcd_OnValidCcdAmount_ReturnsCorrectValue(ulong amount)
    {
        var ccdAmount = CcdAmount.FromCcd(amount);
        ccdAmount.Value.Should().Be(CcdAmount.MicroCcdPerCcd * amount);
    }

    [Theory]
    [InlineData((ulong.MaxValue / CcdAmount.MicroCcdPerCcd) + 1)]
    [InlineData((ulong.MaxValue / CcdAmount.MicroCcdPerCcd) + 1024)]
    public void FromCcd_OnTooLargeCcdAmount_ThrowsException(ulong amount)
    {
        Action result = () => CcdAmount.FromCcd(amount);
        result.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    [InlineData(6, 7)]
    public void AddCcdAmounts_ReturnsCorrectValue(ulong amountA, ulong amountB)
    {
        var ccdAmountA = CcdAmount.FromMicroCcd(amountA);
        var ccdAmountB = CcdAmount.FromMicroCcd(amountB);
        (ccdAmountA + ccdAmountB).Value.Should().Be(amountA + amountB);
    }

    [Theory]
    [InlineData(ulong.MaxValue, 1)]
    [InlineData(1, ulong.MaxValue)]
    [InlineData(ulong.MaxValue - 10, ulong.MaxValue - 10)]
    public void AddCcdAmounts_OnTooLargeSum_ThrowsException(ulong amountA, ulong amountB)
    {
        var ccdAmountA = CcdAmount.FromMicroCcd(amountA);
        var ccdAmountB = CcdAmount.FromMicroCcd(amountB);
        Action result = () =>
        {
            var a = ccdAmountA + ccdAmountB;
        };
        result.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 0)]
    [InlineData(10, 5)]
    public void SubtractCcdAmounts_ReturnsCorrectValue(ulong amountA, ulong amountB)
    {
        var ccdAmountA = CcdAmount.FromMicroCcd(amountA);
        var ccdAmountB = CcdAmount.FromMicroCcd(amountB);
        (ccdAmountA - ccdAmountB).Value.Should().Be(amountA - amountB);
    }

    [Theory]
    [InlineData(ulong.MaxValue - 1, ulong.MaxValue)]
    [InlineData(0, 1)]
    [InlineData(5, 10)]
    public void SubtractCcdAmounts_OnNegativeResult_ThrowsException(ulong amountA, ulong amountB)
    {
        var ccdAmountA = CcdAmount.FromMicroCcd(amountA);
        var ccdAmountB = CcdAmount.FromMicroCcd(amountB);
        Action result = () =>
        {
            var a = ccdAmountA - ccdAmountB;
        };
        result.Should().Throw<ArgumentException>();
    }
}
