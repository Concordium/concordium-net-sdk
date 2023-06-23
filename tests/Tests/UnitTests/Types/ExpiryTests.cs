using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class ExpiryTests
{
    [Fact]
    public void WhenGivenSameTime_ThenEquals()
    {
        // Assert
        var first = Expiry.From(1);
        var second = Expiry.From(1);

        // Act & Assert
        first.Should().Be(second);
    }

    [Fact]
    public void WhenGivenSameTime_ThenSameHash()
    {
        // Assert
        var first = Expiry.From(1);
        var second = Expiry.From(1);

        // Act
        var firstHash = first.GetHashCode();
        var secondHash = second.GetHashCode();

        // Assert
        firstHash.Should().Be(secondHash);
    }

    [Fact]
    public void WhenGivenDifferentTime_ThenAbleToCompare()
    {
        // Assert
        var first = Expiry.From(1);
        var second = Expiry.From(2);

        // Act
        var above = second > first;

        // Assert
        above.Should().BeTrue();
    }
}
