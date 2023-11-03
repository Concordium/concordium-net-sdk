using System;
using System.IO;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class ContractEventTests
{
    [Fact]
    public void WhenGetDeserializedEventsFromUpdated_ThenReturnParsedEvents()
    {
        // Arrange
        var schema = File.ReadAllText("./Data/cis2_wCCD_sub").Trim();
        const string contractName = "cis2_wCCD";
        const string eventMessage = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        const string expectedEvent = /*lang=json,strict*/ "{\"Mint\":{\"amount\":\"1000000\",\"owner\":{\"Account\":[\"3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV\"]},\"token_id\":\"\"}}";
        var versionedModuleSchema = new VersionedModuleSchema(schema, ModuleSchemaVersion.Undefined);
        var contractEvent = new ContractEvent(Convert.FromHexString(eventMessage));

        // Act
        var events = contractEvent.GetDeserializeEvent(versionedModuleSchema, contractName);

        // Assert
        events.Should().Be(expectedEvent);
    }
}
