using System;
using System.IO;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class RejectReasonTests
{
    [Fact]
    public void WhenGetDeserializedEventsRejectedReceive_ThenReturnParsedMessage()
    {
        // Arrange
        var schema = File.ReadAllText("./Data/cis2_wCCD_sub").Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        const string message = "005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000";
        const string expectedMessage = "{\"data\":\"\",\"to\":{\"Account\":[\"3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV\"]}}";
        _ = ReceiveName.TryParse($"{contractName}.{entrypoint}", out var result);
        var versionedModuleSchema = new VersionedModuleSchema(schema, ModuleSchemaVersion.Undefined);

        var rejectedReceive = new RejectedReceive(
            -1,
            new ContractAddress(1,0),
            result.ReceiveName!,
            new Parameter(Convert.FromHexString(message)));

        // Act
        var deserializeMessage = rejectedReceive.GetDeserializeMessage(versionedModuleSchema);

        // Assert
        deserializeMessage.Should().Be(expectedMessage);
    }
}
