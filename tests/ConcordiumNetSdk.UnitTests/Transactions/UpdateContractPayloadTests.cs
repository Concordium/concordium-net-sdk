using System;
using System.IO;
using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class UpdateContractPayloadTests
{
    [Fact]
    public void Create_when_valid_data_passed_should_create_correct_instance()
    {
        // Arrange
        var amount = CcdAmount.Zero;
        var contractAddress = ContractAddress.Create(96, 0);
        var receiveName = "contractName.receiveFunc";
        var parameter = UpdateContractParameter.Empty();
        var maxContractExecutionEnergy = 10_000ul;

        // Act
        var updateContractPayload = UpdateContractPayload.Create(
            amount,
            contractAddress,
            receiveName,
            parameter,
            maxContractExecutionEnergy);

        // Assert
        updateContractPayload.Amount.Should().Be(amount);
        updateContractPayload.ContractAddress.Should().Be(contractAddress);
        updateContractPayload.ReceiveName.Should().Be(receiveName);
        updateContractPayload.Parameter.Should().Be(parameter);
        updateContractPayload.MaxContractExecutionEnergy.Should().Be(maxContractExecutionEnergy);
    }

    [Fact]
    public void SerializeToBytes_when_no_parameter_exists_should_return_correct_data()
    {
        // Arrange
        var amount = CcdAmount.Zero;
        var contractAddress = ContractAddress.Create(96, 0);
        var receiveName = "contractName.receiveFunc";
        var parameter = Array.Empty<byte>();
        var maxContractExecutionEnergy = 10_000ul;

        var updateContractPayload = UpdateContractPayload.Create(
            amount,
            contractAddress,
            receiveName,
            UpdateContractParameter.Empty(),
            maxContractExecutionEnergy);
        
        var expectedSerializedUpdateContractPayload = GetExpectedSerializedUpdateContractPayload();

        // Act
        var serializedUpdateContractPayload = updateContractPayload.SerializeToBytes();

        // Assert
        serializedUpdateContractPayload.Should().BeEquivalentTo(expectedSerializedUpdateContractPayload);
    }

    [Fact]
    public void GetBaseEnergyCost_should_return_correct_data()
    {
        // Arrange
        var amount = CcdAmount.Zero;
        var contractAddress = ContractAddress.Create(96, 0);
        var receiveName = "contractName.receiveFunc";
        var maxContractExecutionEnergy = 10_000ul;

        var updateContractPayload = UpdateContractPayload.Create(
            amount,
            contractAddress,
            receiveName,
            UpdateContractParameter.Empty(),
            maxContractExecutionEnergy);

        var expectedBaseEnergyCost = 10000ul;

        // Act
        var baseEnergyCost = updateContractPayload.GetBaseEnergyCost();

        // Assert
        baseEnergyCost.Should().Be(expectedBaseEnergyCost);
    }

    private byte[] GetExpectedSerializedUpdateContractPayload()
    {
        return new byte[]
        {
            2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 96, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 99, 111, 110, 116, 
            114, 97, 99, 116, 78, 97, 109, 101, 46, 114, 101, 99, 101, 105, 118, 101, 70, 117, 110, 99, 0, 0
        };
    }
}
