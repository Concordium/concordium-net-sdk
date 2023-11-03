using System;
using System.IO;
using System.Threading.Tasks;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class VersionedModuleSourceTests
{
    [Theory]
    [InlineData("cis1-wccd-embedded-schema-v0-unversioned.wasm", ModuleSchemaVersion.V0, false, 0)]
    [InlineData("cis1-wccd-embedded-schema-v0-versioned.wasm.v0", ModuleSchemaVersion.Undefined, true, 0)]
    [InlineData("cis2-wccd-embedded-schema-v1-unversioned.wasm.v1", ModuleSchemaVersion.V1, true, 1)]
    [InlineData("cis2-wccd-embedded-schema-v1-versioned.wasm.v1", ModuleSchemaVersion.Undefined, true, 1)]
    public async Task WhenCreateModuleSchema_ThenParseWithCorrectVersion(string fileName, ModuleSchemaVersion version, bool trim, int moduleVersion)
    {
        var bytes = await File.ReadAllBytesAsync($"./Data/{fileName}");
        if (trim)
        {
            bytes = bytes[8..];
        }
        VersionedModuleSource module = moduleVersion == 0 ? new ModuleV0(bytes) : new ModuleV1(bytes);

        // Act
        var moduleSchema = module.GetModuleSchema();

        // Assert
        moduleSchema!.Version.Should().Be(version);
    }


    [Theory]
    [InlineData("module.schema_embedded.wasm.hex", "FFFF03010000000C00000054657374436F6E7472616374000000000001150200000003000000466F6F020300000042617202")]
    [InlineData("module.wasm.hex", null)]
    public async Task WhenCreateModuleSchema_ThenSchemaPresent(string fileName, string? schema)
    {
        // Arrange
        var load = (await File.ReadAllTextAsync($"./Data/{fileName}")).Trim();
        var module = new ModuleV1(Convert.FromHexString(load));

        // Act
        var moduleSchema = module.GetModuleSchema();

        // Assert
        if (schema is not null)
        {
            moduleSchema.Should().NotBeNull();
            moduleSchema!.Schema.Should().Be(schema.ToLowerInvariant());
        }
        else
        {
            moduleSchema.Should().BeNull();
        }
    }
}
