using System;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class AccountSequenceNumberTests
{
    [Fact]
    public void Same_SequenceNumbers_AreEqual()
    {
        var sequenceNumberA = AccountSequenceNumber.From(1);
        var sequenceNumberB = AccountSequenceNumber.From(1);
        Assert.Equal(sequenceNumberA, sequenceNumberB);
    }

    [Fact]
    public void Different_SequenceNumbers_AreNotEqual()
    {
        var sequenceNumberA = AccountSequenceNumber.From(1);
        var sequenceNumberB = AccountSequenceNumber.From(2);
        Assert.NotEqual(sequenceNumberA, sequenceNumberB);
    }

    [Fact]
    public void GetIncrementedSequenceNumber_OnSufficientlySmallSequenceNumber_ReturnsIncremented()
    {
        var sequenceNumberA = AccountSequenceNumber.From(1).GetIncrementedSequenceNumber();
        var sequenceNumberB = AccountSequenceNumber.From(2);
        Assert.Equal(sequenceNumberA, sequenceNumberB);
    }

    [Fact]
    public void From_Zero_ThrowsException()
    {
        Action result = () => AccountSequenceNumber.From(0);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetIncrementedSequenceNumber_OnMaxValuedAccountSequenceNumber_ThrowsException()
    {
        var sequenceNumber = AccountSequenceNumber.From(ulong.MaxValue);
        Action result = () => sequenceNumber.GetIncrementedSequenceNumber();
        result.Should().Throw<OverflowException>();
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var sequenceNumber = AccountSequenceNumber.From(100);
        var expectedSerializedSequenceNumber = new byte[] { 0, 0, 0, 0, 0, 0, 0, 100 };
        var serializedSequenceNumber = sequenceNumber.GetBytes();
        serializedSequenceNumber.Should().BeEquivalentTo(expectedSerializedSequenceNumber);
    }
}
