using System;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class CcdAmountTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(UInt64.MaxValue / CcdAmount.MicroCcdPerCcd)]
    public void FromCcd_OnValidCcdAmount_ReturnsCorrectValue(UInt64 amount)
    {
        var ccdAmount = CcdAmount.FromCcd(amount);
        ccdAmount.Value.Should().Be(CcdAmount.MicroCcdPerCcd * amount);
    }

    [Theory]
    [InlineData((UInt64.MaxValue / CcdAmount.MicroCcdPerCcd) + 1)]
    [InlineData((UInt64.MaxValue / CcdAmount.MicroCcdPerCcd) + 1024)]
    public void FromCcd_OnTooLargeCcdAmount_ThrowsException(UInt64 amount)
    {
        Action result = () => CcdAmount.FromCcd(amount);
        result.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    [InlineData(6, 7)]
    public void AddCcdAmounts_ReturnsCorrectValue(UInt64 amountA, UInt64 amountB)
    {
        var ccdAmountA = CcdAmount.FromMicroCcd(amountA);
        var ccdAmountB = CcdAmount.FromMicroCcd(amountB);
        (ccdAmountA + ccdAmountB).Value.Should().Be(amountA + amountB);
    }

    [Theory]
    [InlineData(UInt64.MaxValue, 1)]
    [InlineData(1, UInt64.MaxValue)]
    [InlineData(UInt64.MaxValue - 10, UInt64.MaxValue - 10)]
    public void AddCcdAmounts_OnTooLargeSum_ThrowsException(UInt64 amountA, UInt64 amountB)
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
    public void SubtractCcdAmounts_ReturnsCorrectValue(UInt64 amountA, UInt64 amountB)
    {
        var ccdAmountA = CcdAmount.FromMicroCcd(amountA);
        var ccdAmountB = CcdAmount.FromMicroCcd(amountB);
        (ccdAmountA - ccdAmountB).Value.Should().Be(amountA - amountB);
    }

    [Theory]
    [InlineData(UInt64.MaxValue - 1, UInt64.MaxValue)]
    [InlineData(0, 1)]
    [InlineData(5, 10)]
    public void SubtractCcdAmounts_OnNegativeResult_ThrowsException(UInt64 amountA, UInt64 amountB)
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
