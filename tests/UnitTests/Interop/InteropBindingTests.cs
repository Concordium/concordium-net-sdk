using System.IO;
using System.Threading.Tasks;
using Concordium.Sdk.Interop;
using Concordium.Sdk.Types;
using FluentAssertions;
using VerifyXunit;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Interop;

[UsesVerify]
public class InteropBindingTests
{
    [Fact]
    public async Task GivenSchemaVersion_WhenSchemaDisplay_ThenReturnSchema()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2-nft-schema")).Trim();

        // Act
        var message = InteropBinding.SchemaDisplay(schema, ModuleSchemaVersion.V1);

        // Assert
        await Verifier.Verify(message)
            .UseFileName("module-versioned-schema")
            .UseDirectory("__snapshots__");
    }

    [Fact]
    public async Task WhenSchemaDisplay_ThenReturnSchema()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();

        // Act
        var message = InteropBinding.SchemaDisplay(schema,  ModuleSchemaVersion.Undefined);

        // Assert
        await Verifier.Verify(message)
            .UseFileName("module-schema")
            .UseDirectory("__snapshots__");
    }

    [Fact]
    public async Task WhenDisplayReceiveParam_ThenReturnParams()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        const string value = "005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000";

        // Act
        var message = InteropBinding.GetReceiveContractParameter(schema, contractName, entrypoint, value,null);

        // Assert
        await Verifier.Verify(message)
            .UseFileName("receive-params")
            .UseDirectory("__snapshots__");
    }

    [Fact]
    public async Task WhenDisplayEvent_ThenReturnEvent()
    {
        // Arrange
        var schema = (await File.ReadAllTextAsync("./Data/cis2_wCCD_sub")).Trim();
        const string contractName = "cis2_wCCD";
        const string value = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";

        // Act
        var message = InteropBinding.GetEventContract(schema, contractName, value, ModuleSchemaVersion.Undefined);

        // Assert
        await Verifier.Verify(message)
            .UseFileName("event")
            .UseDirectory("__snapshots__");
    }

    [Theory]
    [InlineData(ModuleSchemaVersion.V0, (byte)0)]
    [InlineData(ModuleSchemaVersion.V2, (byte)2)]
    public void GivenVersion_WhenMapModuleSchemaVersionToFFIOption_ThenOptionContainsVersion(ModuleSchemaVersion version, byte mapped)
    {
        // Act
        var ffiOption = InteropBinding.FFIByteOption.Create(version);

        // Assert
        ffiOption.is_some.Should().Be(1);
        ffiOption.t.Should().Be(mapped);
    }

    [Fact]
    public void GivenUndefinedVersion_WhenMapModuleSchemaVersionToFFIOption_ThenOptionEmpty()
    {
        // Act
        var ffiOption = InteropBinding.FFIByteOption.Create(ModuleSchemaVersion.Undefined);

        // Assert
        ffiOption.is_some.Should().Be(0);
    }
}
