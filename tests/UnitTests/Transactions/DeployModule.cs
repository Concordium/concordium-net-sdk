using System;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Helpers;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Transactions;

public sealed class DeployModuleTests
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeployModule"/>
    /// transaction with a short WASM source
    /// </summary>
    public static DeployModule CreateDeployModule()
    {
        var source = new byte[] {
            0, 97, 115, 109, 1, 0, 0, 0, 4, 5, 1, 112, 1, 1, 1, 5, 3, 1,
            0, 16, 6, 25, 3, 127, 1, 65, 128, 128, 192, 0, 11, 127, 0, 65,
            128, 128, 192, 0, 11, 127, 0, 65, 128, 128, 192, 0, 11, 7, 37,
            3, 6, 109, 101, 109, 111, 114, 121, 2, 0, 10, 95, 95, 100, 97,
            116, 97, 95, 101, 110, 100, 3, 1, 11, 95, 95, 104, 101, 97, 112,
            95, 98, 97, 115, 101, 3, 2
        };
        var module = ModuleV1.From(source);
        return new DeployModule(module);
    }

    [Fact]
    public void ToBytes_ReturnsCorrectValue()
    {
        // The expected payload was generated using the Concordium Rust SDK.
        var expectedBytes = new byte[] { 
            0, 0, 0, 0, 1, 0, 0, 0, 86, 0, 97, 115, 109, 1, 0, 0, 0, 4, 5,
            1, 112, 1, 1, 1, 5, 3, 1, 0, 16, 6, 25, 3, 127, 1, 65, 128, 128,
            192, 0, 11, 127, 0, 65, 128, 128, 192, 0, 11, 127, 0, 65, 128,
            128, 192, 0, 11, 7, 37, 3, 6, 109, 101, 109, 111, 114, 121, 2,
            0, 10, 95, 95, 100, 97, 116, 97, 95, 101, 110, 100, 3, 1, 11,
            95, 95, 104, 101, 97, 112, 95, 98, 97, 115, 101, 3, 2
        };

        CreateDeployModule().ToBytes().Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public void ToBytes_InverseOfFromBytes()
    {
        // The expected payload was generated using the Concordium Rust SDK.
        var moduleBytes = CreateDeployModule().ToBytes();

        (DeployModule?, DeserialErr?) module = (null, null);
        var deserialSuccess = DeployModule.TryDeserial(moduleBytes, out module);

        CreateDeployModule().Should().BeEquivalentTo(module.Item1);
    }
}
