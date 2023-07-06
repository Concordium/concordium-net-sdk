using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class ReceiveNameTests
{
    [Theory]
    [InlineData("example.name", true, null)] // null name
    [InlineData("", false, ReceiveName.ValidationError.MissingDotSeparator)] // empty name
    [InlineData("invalid", false, ReceiveName.ValidationError.MissingDotSeparator)] // name without dot
    [InlineData("contract_with_a_very_long_name_that_exceeds_the_maximum_allowed_length.contract_with_a_very_long_name_that_exceeds_the_maximum_allowed_length.receive", false, ReceiveName.ValidationError.TooLong)] // name longer than MaxByteLength
    [InlineData("invalid.💡name", false, ReceiveName.ValidationError.InvalidCharacters)] // name with invalid character
    public void WhenCallingTryParse_ValidatesAndParsesReceiveName(
        string name,
        bool expectedResult,
        ReceiveName.ValidationError? expectedError)
    {
        // Act
        var result = ReceiveName.TryParse(name, out var output);
        var (receiveName, error) = output;

        // Assert
        result.Should().Be(expectedResult);
        if (expectedResult)
        {
            receiveName.Should().NotBeNull();
            receiveName!.Receive.Should().Be(name);
            error.Should().BeNull();
        }
        else
        {
            error.Should().NotBeNull();
            error!.Should().Be(expectedError);
            receiveName.Should().BeNull();
        }
    }
}
