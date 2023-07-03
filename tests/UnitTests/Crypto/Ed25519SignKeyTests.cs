using Concordium.Sdk.Crypto;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Crypto;

public class Ed25519SignKeyTests
{
    [Fact]
    public void WhenUsingSameKey_ThenEquals()
    {
        // Arrange
        var keyOne = Ed25519SignKey.From("56f60de843790c308dac2d59a5eec9f6b1649513f827e5a13d7038accfe31784");
        var keySecond = Ed25519SignKey.From("56f60de843790c308dac2d59a5eec9f6b1649513f827e5a13d7038accfe31784");

        // Act
        var equals = keyOne == keySecond;

        // Assert
        equals.Should().BeTrue();
    }
}
