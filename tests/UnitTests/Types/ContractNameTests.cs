using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class ContractNameTests
{
    [Theory]
    [InlineData("init_contract", true, null)] // Valid name
    [InlineData("init_contract.", false, ContractName.ValidationError.ContainsDot)] // Contains '.'
    [InlineData("init_contract_with_a_very_long_name_that_exceeds_the_maximum_allowed_lengthcontract_with_a_very_long_name_that_exceeds_the_maximum_allowed_length", false, ContractName.ValidationError.TooLong)] // Exceeds maximum length
    [InlineData("init_contract$", true, null)] // Contains ASCII punctuation
    [InlineData("init_contract💡", false, ContractName.ValidationError.InvalidCharacters)] // Contains non-ASCII character
    [InlineData("init_contract ", false, ContractName.ValidationError.InvalidCharacters)] // Contains whitespace
    [InlineData("init_ contract", false, ContractName.ValidationError.InvalidCharacters)] // Contains whitespace
    [InlineData("contract", false, ContractName.ValidationError.MissingInitPrefix)] // Does not start with 'init_'
    public void WhenCallingTryParse_ValidatesAndParsesReceiveName(
        string name,
        bool expectedResult,
            ContractName.ValidationError? expectedError)
    {
        // Act
        var result = ContractName.TryParse(name, out var output);
        var (initName, error) = output;

        // Assert
        result.Should().Be(expectedResult);
        if (expectedResult)
        {
            initName.Should().NotBeNull();
            initName!.Name.Should().Be(name);
            error.Should().BeNull();
        }
        else
        {
            error.Should().NotBeNull();
            error!.Should().Be(expectedError);
            initName.Should().BeNull();
        }
    }

    [Fact]
    public void WhenGetContractNamePart_ThenReturnContractName()
    {
        // Arrange
        const string prefix = "init_";
        const string expected = "awesome";
        _ = ContractName.TryParse($"{prefix}{expected}", out var result);

        // Act
        var actual = result.ContractName!.GetContractName();

        // Assert
        actual.ContractName.Should().Be(expected);
    }

    [Fact]
    public void DeserializesCorrectly()
    {
        var success = ContractName.TryParse("init_name", out var parsed);
        if (!success) {
            Assert.Fail(parsed.Error.ToString());
        }

        var bytes = new byte[] {
            0,
            9,
            105,
            110,
            105,
            116,
            95,
            110,
            97,
            109,
            101
        };
        Assert.Equal(parsed.ContractName!.ToBytes(), bytes);
        Assert.Equal(parsed.ContractName!.SerializedLength(), (uint)bytes.Length);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var parseSuccess = ContractName.TryParse("init_some_name", out var parsed);
        if (!parseSuccess) {
            Assert.Fail(parsed.Error.ToString());
        }
        var deserialSuccess = ContractName.TryDeserial(parsed.ContractName!.ToBytes(), out var deserial);
        if (!deserialSuccess) {
            Assert.Fail(deserial.Error);
        }
        deserial.ContractName.Should().Be(parsed.ContractName);
        Assert.Equal(parsed.ContractName!.SerializedLength(), (uint)parsed.ContractName!.ToBytes().Length);
    }
}
