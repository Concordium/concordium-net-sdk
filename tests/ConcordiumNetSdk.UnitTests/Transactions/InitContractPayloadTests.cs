using System;
using System.IO;
using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class InitContractPayloadTests
{
    [Fact]
    public void Create_when_valid_data_passed_should_create_correct_instance()
    {
        // Arrange
        var amount = CcdAmount.Zero;
        var moduleRef = ModuleRef.From("2c490881e1f87e46cac9a9419f1c669d5f6d74eabc3c5a4fd10d13ab29f8b8c2");
        var initName = "init_name";
        var parameter = InitContractParameter.Empty();
        var maxContractExecutionEnergy = 10_000ul;

        // Act
        var initContractPayload = InitContractPayload.Create(
            amount,
            moduleRef,
            initName,
            parameter,
            maxContractExecutionEnergy);

        // Assert
        initContractPayload.Amount.Should().Be(amount);
        initContractPayload.ModuleRef.Should().Be(moduleRef);
        initContractPayload.InitName.Should().Be(initName);
        initContractPayload.Parameter.Should().Be(parameter);
        initContractPayload.MaxContractExecutionEnergy.Should().Be(maxContractExecutionEnergy);
    }

    [Fact]
    public void SerializeToBytes_when_no_parameter_exists_should_return_correct_data()
    {
        // Arrange
        var amount = CcdAmount.Zero;
        var moduleRef = ModuleRef.From("2c490881e1f87e46cac9a9419f1c669d5f6d74eabc3c5a4fd10d13ab29f8b8c2");
        var initName = "init_name";
        var parameter = Array.Empty<byte>();
        var maxContractExecutionEnergy = 10_000ul;

        var initContractPayload = InitContractPayload.Create(
            amount,
            moduleRef,
            initName,
            InitContractParameter.Empty(),
            maxContractExecutionEnergy);
        
        var expectedSerializedInitContractPayload = GetExpectedSerializedInitContractPayload();

        // Act
        var serializedInitContractPayload = initContractPayload.SerializeToBytes();

        // Assert
        serializedInitContractPayload.Should().BeEquivalentTo(expectedSerializedInitContractPayload);
    }

    [Fact]
    public void GetBaseEnergyCost_should_return_correct_data()
    {
        // Arrange
        var amount = CcdAmount.Zero;
        var moduleRef = ModuleRef.From("2c490881e1f87e46cac9a9419f1c669d5f6d74eabc3c5a4fd10d13ab29f8b8c2");
        var initName = "init_name";
        var parameter = Array.Empty<byte>();
        var maxContractExecutionEnergy = 10_000ul;

        var initContractPayload = InitContractPayload.Create(
            amount,
            moduleRef,
            initName,
            InitContractParameter.Empty(),
            maxContractExecutionEnergy);

        var expectedBaseEnergyCost = 10000ul;

        // Act
        var baseEnergyCost = initContractPayload.GetBaseEnergyCost();

        // Assert
        baseEnergyCost.Should().Be(expectedBaseEnergyCost);
    }

    private byte[] GetExpectedSerializedInitContractPayload()
    {
        return new byte[]
        {
            1, 0, 0, 0, 0, 0, 0, 0, 0, 44, 73, 8, 129, 225, 248, 126, 70, 202, 201, 169, 65, 159, 28, 102, 157, 95, 109,
            116, 234, 188, 60, 90, 79, 209, 13, 19, 171, 41, 248, 184, 194, 0, 9, 105, 110, 105, 116, 95, 110, 97, 109,
            101, 0, 0
        };
    }
}
