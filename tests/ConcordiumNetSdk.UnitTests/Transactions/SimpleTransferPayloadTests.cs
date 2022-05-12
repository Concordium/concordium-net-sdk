using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class SimpleTransferPayloadTests
{
    [Fact]
    public void Create_when_valid_data_passed_should_create_correct_instance()
    {
        // Arrange
        var ccdAmount = CcdAmount.FromCcd(100);
        var toAccountAddress = AccountAddress.From("3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw");

        // Act
        var simpleTransferPayload = SimpleTransferPayload.Create(ccdAmount, toAccountAddress);

        // Assert
        simpleTransferPayload.Amount.Should().Be(ccdAmount);
        simpleTransferPayload.ToAddress.Should().Be(toAccountAddress);
    }

    [Fact]
    public void SerializeToBytes_should_return_correct_data()
    {
        // Arrange
        var ccdAmount = CcdAmount.FromCcd(100);
        var toAccountAddress = AccountAddress.From("3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw");
        var simpleTransferPayload = SimpleTransferPayload.Create(ccdAmount, toAccountAddress);
        var expectedSerializedBytes = new byte[]
        {
            3, 71, 16, 92, 61, 132, 191, 45, 174, 170, 208, 206, 153, 215, 123, 117, 254, 225, 53, 137,
            184, 94, 41, 112, 215, 225, 165, 254, 29, 145, 253, 190, 160, 0, 0, 0, 0, 5, 245, 225, 0
        };

        // Act
        var serializedBytes = simpleTransferPayload.SerializeToBytes();

        // Assert
        serializedBytes.Should().BeEquivalentTo(expectedSerializedBytes);
    }

    [Fact]
    public void GetBaseEnergyCost_should_return_correct_data()
    {
        // Arrange
        var ccdAmount = CcdAmount.FromCcd(100);
        var toAccountAddress = AccountAddress.From("3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw");
        var simpleTransferPayload = SimpleTransferPayload.Create(ccdAmount, toAccountAddress);
        var expectedBaseEnergyCost = 300;

        // Act
        var baseEnergyCost = simpleTransferPayload.GetBaseEnergyCost();

        // Assert
        baseEnergyCost.Should().Be(expectedBaseEnergyCost);
    }
}
