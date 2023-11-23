using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class ContractInitializedEventTests
{
    [Fact]
    public void WhenGetDeserializedEventsFromContractInitializedEvent_ThenReturnParsedEvents()
    {
        // Arrange
        var schema = File.ReadAllText("./Data/cis2_wCCD_sub").Trim();
        const string contractName = "cis2_wCCD";
        const string eventMessage = "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";
        const string expectedEvent = /*lang=json,strict*/ "{\"Mint\":{\"amount\":\"1000000\",\"owner\":{\"Account\":[\"3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV\"]},\"token_id\":\"\"}}";
        _ = ContractName.TryParse($"init_{contractName}", out var result);
        var versionedModuleSchema = VersionedModuleSchema.Create(schema, ModuleSchemaVersion.Undefined);

        var contractInitializedEvent = new ContractInitializedEvent(
            ContractVersion.V0,
            new ModuleReference(Convert.ToHexString(new byte[Hash.BytesLength])),
            new ContractAddress(1, 0),
            CcdAmount.Zero,
            result.ContractName!,
            new List<ContractEvent> { new(Convert.FromHexString(eventMessage)) });

        // Act
        var events = contractInitializedEvent.GetDeserializedEvents(versionedModuleSchema);

        // Assert
        events.Select(e => e.ToString()).Should().BeEquivalentTo(new List<string> { expectedEvent });
    }
}
