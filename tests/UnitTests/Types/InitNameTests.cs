using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class InitNameTests
{
    [Theory]
    [InlineData("init_contract", true)] // Valid name
    [InlineData("init_contract.", false)] // Contains '.'
    [InlineData("init_contract_with_a_very_long_name_that_exceeds_the_maximum_allowed_lengthcontract_with_a_very_long_name_that_exceeds_the_maximum_allowed_length", false)] // Exceeds maximum length
    [InlineData("init_contract$", true)] // Contains ASCII punctuation
    [InlineData("init_contract💡", false)] // Contains non-ASCII character
    [InlineData("init_contract ", false)] // Contains whitespace
    [InlineData("init_ contract", false)] // Contains whitespace
    [InlineData("contract", false)] // Does not start with 'init_'
    public void WhenCallingTryParse_ValidatesAndParsesReceiveName(string name, bool expectedResult)
    {
        // Act
        var result = InitName.TryParse(name, out var initName);

        // Assert
        result.Should().Be(expectedResult);
        if (expectedResult)
        {
            initName.Should().NotBeNull();
            initName!.Name.Should().Be(name);
        }
        else
        {
            initName.Should().BeNull();
        }
    }
}
