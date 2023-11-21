using System;
using System.Collections.Generic;
using System.IO;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class ContractTraceElementTests
{
    [Fact]
    public void WhenGetDeserializedMessageFromUpdated_ThenReturnParsedMessage()
    {
        // Arrange
        var schema = File.ReadAllText("./Data/cis2_wCCD_sub").Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        const string message = "005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000";
        const string expectedMessage = /*lang=json,strict*/ "{\"data\":\"\",\"to\":{\"Account\":[\"3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV\"]}}";
        _ = ReceiveName.TryParse($"{contractName}.{entrypoint}", out var result);
        var versionedModuleSchema = VersionedModuleSchema.Create(schema, ModuleSchemaVersion.Undefined);

        var updated = new Updated(
            ContractVersion.V0,
            new ContractAddress(1, 0),
            new ContractAddress(1, 0),
            CcdAmount.Zero,
            new Parameter(Convert.FromHexString(message)),
            result.ReceiveName!,
            new List<ContractEvent>());

        // Act
        var deserializeMessage = updated.GetDeserializeMessage(versionedModuleSchema);

        // Assert
        deserializeMessage.Should().Be(expectedMessage);
    }

    [Fact]
    public void WhenGetDeserializedEventsFromUpdated_ThenReturnParsedEvents()
    {
        // Arrange
        var schema = File.ReadAllText("./Data/cis2_wCCD_sub").Trim();
        const string contractName = "cis2_wCCD";
        const string entrypoint = "wrap";
        const string eventMessage = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        const string expectedEvent = /*lang=json,strict*/ "{\"Mint\":{\"amount\":\"1000000\",\"owner\":{\"Account\":[\"3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV\"]},\"token_id\":\"\"}}";
        _ = ReceiveName.TryParse($"{contractName}.{entrypoint}", out var result);
        var versionedModuleSchema = VersionedModuleSchema.Create(schema, ModuleSchemaVersion.Undefined);

        var updated = new Updated(
            ContractVersion.V0,
            new ContractAddress(1, 0),
            new ContractAddress(1, 0),
            CcdAmount.Zero,
            new Parameter(Array.Empty<byte>()),
            result.ReceiveName!,
            new List<ContractEvent> { new(Convert.FromHexString(eventMessage)) });

        // Act
        var events = updated.GetDeserializedEvents(versionedModuleSchema);

        // Assert
        events.Should().BeEquivalentTo(new List<string> { expectedEvent });
    }

    [Fact]
    public void WhenGetDeserializedEventsFromInterrupted_ThenReturnParsedEvents()
    {
        // Arrange
        var schema = File.ReadAllText("./Data/cis2_wCCD_sub").Trim();
        const string contractName = "cis2_wCCD";
        const string eventMessage = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        const string expectedEvent = /*lang=json,strict*/ "{\"Mint\":{\"amount\":\"1000000\",\"owner\":{\"Account\":[\"3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV\"]},\"token_id\":\"\"}}";
        var versionedModuleSchema = VersionedModuleSchema.Create(schema, ModuleSchemaVersion.Undefined);

        var interrupted = new Interrupted(
            new ContractAddress(1, 0),
            new List<ContractEvent> { new(Convert.FromHexString(eventMessage)) });

        // Act
        var events = interrupted.GetDeserializedEvents(versionedModuleSchema, new ContractIdentifier(contractName));

        // Assert
        events.Should().BeEquivalentTo(new List<string> { expectedEvent });
    }
}
