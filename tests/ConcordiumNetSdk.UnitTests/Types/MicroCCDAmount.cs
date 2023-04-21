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
        var ccdAmount = MicroCCDAmount.Zero();
        ccdAmount.GetMicroCcdValue().Should().Be(0);
    }

    [Fact]
    public void FromMicroCcd_when_valid_ulong_value_passed_should_create_correct_instance()
    {
        var value = UInt64.MaxValue;
        var ccdAmount = MicroCCDAmount.FromMicroCcd(value);
        ccdAmount.GetMicroCcdValue().Should().Be(value);
    }

    [Fact]
    public void FromMicroCcd_when_valid_int_value_passed_should_create_correct_instance()
    {
        var value = UInt64.MaxValue;
        var ccdAmount = MicroCCDAmount.FromMicroCcd(value);
        ccdAmount.GetMicroCcdValue().Should().Be(value);
    }

    [Fact]
    public void FromCcd_when_valid_value_passed_should_create_correct_instance()
    {
        var value = UInt64.MaxValue;
        var ccdAmount = MicroCCDAmount.FromCcd(value);
        ccdAmount.GetMicroCcdValue().Should().Be((ulong)value * 1_000_000);
    }
}
