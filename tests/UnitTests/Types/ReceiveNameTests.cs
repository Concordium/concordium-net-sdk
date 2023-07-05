using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class ReceiveNameTests
{
    [Theory]
    [InlineData("example.name", true)] // null name
    [InlineData("", false)] // empty name
    [InlineData("invalid", false)] // name without dot
    [InlineData("contract_with_a_very_long_name_that_exceeds_the_maximum_allowed_length.contract_with_a_very_long_name_that_exceeds_the_maximum_allowed_length.receive", false)] // name longer than MaxByteLength
    [InlineData("invalid@name", false)] // name with invalid character
    public void WhenCallingTryParse_ValidatesAndParsesReceiveName(string name, bool expectedResult)
    {
        // Act
        var result = ReceiveName.TryParse(name, out var receiveName);

        // Assert
        result.Should().Be(expectedResult);
        if (expectedResult)
        {
            receiveName.Should().NotBeNull();
            receiveName!.Receive.Should().Be(name);
        }
        else
        {
            receiveName.Should().BeNull();
        }
    }
}
