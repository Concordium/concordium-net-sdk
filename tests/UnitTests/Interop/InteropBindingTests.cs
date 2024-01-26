using System;
using System.IO;
using System.Threading.Tasks;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Interop;
using Concordium.Sdk.Types;
using FluentAssertions;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Concordium.Sdk.Tests.UnitTests.Interop;

[UsesVerify]
public class InteropBindingTests
{
    private readonly ITestOutputHelper _outputHelper;

    public InteropBindingTests(ITestOutputHelper outputHelper) => this._outputHelper = outputHelper;

    [Fact]
    public async Task GivenSchemaVersion_WhenSchemaDisplay_ThenReturnSchema()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2-nft-schema")).Trim();
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.V1);

        // Act
        var message = InteropBinding.SchemaDisplay(versionedModuleSchema);

        // Assert
        await Verifier.Verify(message.ToString())
            .UseFileName("module-versioned-schema")
            .UseDirectory("__snapshots__");
    }

    [Fact]
    public async Task WhenSchemaDisplay_ThenReturnSchema()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);

        // Act
        var message = InteropBinding.SchemaDisplay(versionedModuleSchema);

        // Assert
        await Verifier.Verify(message.ToString())
            .UseFileName("module-schema")
            .UseDirectory("__snapshots__");
    }

    [Fact]
    public async Task GivenBadSchema_WhenSchemaDisplay_ThenThrowExceptionWithParseError()
    {
        // Arrange
        var schema = Convert.FromHexString((await File.ReadAllTextAsync("./Data/cis2-nft-schema")).Trim());
        var versionedModuleSchema = new VersionedModuleSchema(schema[..^3], ModuleSchemaVersion.V1); // Bad schema

        // Act

        var action = () => InteropBinding.SchemaDisplay(versionedModuleSchema);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.VersionedSchemaErrorParseError &&
                e.Message.Equals("Parse error", StringComparison.Ordinal));
    }

    [Fact]
    public async Task WhenDisplayReceiveParam_ThenReturnParams()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        const string value = "005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000";
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);
        var parameter = new Parameter(Convert.FromHexString(value));
        var contractIdentifier = new ContractIdentifier(contractName);
        var entryPoint = new EntryPoint(entrypoint);

        // Act
        var message = InteropBinding.GetReceiveContractParameter(versionedModuleSchema, contractIdentifier, entryPoint, parameter);

        // Assert
        await Verifier.Verify(message.ToString())
            .UseFileName("receive-params")
            .UseDirectory("__snapshots__");
    }

    [Fact]
    public async Task GivenBadReceiveParam_WhenDisplayReceiveParam_ThenThrowException()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        var value = Convert.FromHexString("005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000");
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);
        var parameter = new Parameter(value[..^3]); // Bad parameter
        var contractIdentifier = new ContractIdentifier(contractName);
        var entryPoint = new EntryPoint(entrypoint);

        try
        {
            var receiveContractParameter = InteropBinding.GetReceiveContractParameter(versionedModuleSchema, contractIdentifier, entryPoint, parameter);
        }
        catch (InteropBindingException e)
        {
            this._outputHelper.WriteLine($"This is some result: {e.Result}");
        }

        // Act
        var action = () => InteropBinding.GetReceiveContractParameter(versionedModuleSchema, contractIdentifier, entryPoint, parameter);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.JsonError &&
                e.Message.StartsWith("Failed to deserialize AccountAddress due to: Could not parse AccountAddress", StringComparison.InvariantCulture));
    }

    [Fact]
    public async Task GivenBadContractIdentifier_WhenDisplayReceiveParam_ThenThrowException()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);
        var parameter = new Parameter(Convert.FromHexString("005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000"));
        var contractIdentifier = new ContractIdentifier(contractName[..^3]); // Bad contract identifier
        var entryPoint = new EntryPoint(entrypoint);

        // Act
        var action = () => InteropBinding.GetReceiveContractParameter(versionedModuleSchema, contractIdentifier, entryPoint, parameter);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.VersionedSchemaErrorNoContractInModule &&
                e.Message.Equals("Unable to find contract schema in module schema", StringComparison.Ordinal));
    }

    [Fact]
    public async Task GivenBadEntrypoint_WhenDisplayReceiveParam_ThenThrowException()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);
        var parameter = new Parameter(Convert.FromHexString("005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000"));
        var contractIdentifier = new ContractIdentifier(contractName);
        var entryPoint = new EntryPoint(entrypoint[..^2]); // Bad entrypoint

        // Act
        var action = () => InteropBinding.GetReceiveContractParameter(versionedModuleSchema, contractIdentifier, entryPoint, parameter);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.VersionedSchemaErrorNoReceiveInContract &&
                e.Message.Equals("Receive function schema not found in contract schema", StringComparison.Ordinal));
    }

    [Fact]
    public async Task GivenBadSchema_WhenDisplayReceiveParam_ThenThrowException()
    {
        // Arrange
        var schema = Convert.FromHexString((await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim());
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        var versionedModuleSchema = new VersionedModuleSchema(schema[30..], ModuleSchemaVersion.Undefined); // Bad schema
        var parameter = new Parameter(Convert.FromHexString("005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000"));
        var contractIdentifier = new ContractIdentifier(contractName);
        var entryPoint = new EntryPoint(entrypoint[..^2]);

        // Act
        var action = () => InteropBinding.GetReceiveContractParameter(versionedModuleSchema, contractIdentifier, entryPoint, parameter);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.VersionedSchemaErrorMissingSchemaVersion &&
                e.Message.Equals("Missing Schema Version", StringComparison.Ordinal));
    }

    [Fact]
    public async Task WhenDisplayEvent_ThenReturnEvent()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string value = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);
        var contractIdentifier = new ContractIdentifier(contractName);
        var contractEvent = new ContractEvent(Convert.FromHexString(value));

        // Act
        var message = InteropBinding.GetEventContract(versionedModuleSchema, contractIdentifier, contractEvent);

        // Assert
        await Verifier.Verify(message.ToString())
            .UseFileName("event")
            .UseDirectory("__snapshots__");
    }

    [Fact]
    public async Task GivenBadSchema_WhenDisplayEvent_ThenThrowException()
    {
        // Arrange
        var schema = Convert.FromHexString((await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim());
        const string contractName = "cis2_wCCD";
        const string value = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        var versionedModuleSchema = new VersionedModuleSchema(schema[..^3], ModuleSchemaVersion.Undefined); // Bad schema
        var contractIdentifier = new ContractIdentifier(contractName);
        var contractEvent = new ContractEvent(Convert.FromHexString(value));

        // Act
        var action = () =>  InteropBinding.GetEventContract(versionedModuleSchema, contractIdentifier, contractEvent);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.VersionedSchemaErrorMissingSchemaVersion &&
                e.Message.Equals("Missing Schema Version", StringComparison.Ordinal));
    }

    [Fact]
    public async Task GivenBadContractIdentifier_WhenDisplayEvent_ThenThrowException()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string value = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);
        var contractIdentifier = new ContractIdentifier(contractName[..^3]); // Bad contract identifier
        var contractEvent = new ContractEvent(Convert.FromHexString(value));

        // Act
        var action = () =>  InteropBinding.GetEventContract(versionedModuleSchema, contractIdentifier, contractEvent);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.VersionedSchemaErrorNoContractInModule &&
                e.Message.Equals("Unable to find contract schema in module schema", StringComparison.Ordinal));
    }

    [Fact]
    public async Task GivenBadContractEvent_WhenDisplayEvent_ThenThrowException()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string value = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        var versionedModuleSchema = new VersionedModuleSchema(Convert.FromHexString(schema), ModuleSchemaVersion.Undefined);
        var contractIdentifier = new ContractIdentifier(contractName);
        var contractEvent = new ContractEvent(Convert.FromHexString(value)[..^3]); // Bad contract event

        // Act
        var action = () =>  InteropBinding.GetEventContract(versionedModuleSchema, contractIdentifier, contractEvent);

        // Assert
        action.Should().Throw<InteropBindingException>()
            .Where(e =>
                e.Result == Result.JsonError &&
                e.Message.StartsWith("Failed to deserialize AccountAddress due to: Could not parse"));
    }

    [Theory]
    [InlineData(ModuleSchemaVersion.V0, (byte)0)]
    [InlineData(ModuleSchemaVersion.V2, (byte)2)]
    public void GivenVersion_WhenMapModuleSchemaVersionToFFIOption_ThenOptionContainsVersion(ModuleSchemaVersion version, byte mapped)
    {
        // Act
        var ffiOption = InteropBinding.FfiByteOption.Create(version);

        // Assert
        ffiOption.IsSome.Should().Be(1);
        ffiOption.T.Should().Be(mapped);
    }

    [Fact]
    public void GivenUndefinedVersion_WhenMapModuleSchemaVersionToFFIOption_ThenOptionEmpty()
    {
        // Act
        var ffiOption = InteropBinding.FfiByteOption.Create(ModuleSchemaVersion.Undefined);

        // Assert
        ffiOption.IsSome.Should().Be(0);
    }
}
