using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Transactions;

public sealed class InitContractTests
{
    /// <summary>
    /// Creates a new instance of the <see cref="InitContract"/>
    /// transaction.
    /// </summary>
    public static InitContract NewInitContract()
    {
        var amount = CcdAmount.FromCcd(100);
        var moduleRef = new ModuleReference("0000000000000000000000000000000000000000000000000000000000000000");
        var contractName = ContractName.TryParse("init_name", out var parsed);
        var parameter = new Parameter(System.Array.Empty<byte>());

        return new InitContract(amount, moduleRef, parsed.ContractName!, parameter);
    }

    [Fact]
    public void ToBytes_ReturnsCorrectValue()
    {
        // The expected payload was generated using the Concordium Rust SDK.
        var expectedBytes = new byte[]
        {
            1,
            0,
            0,
            0,
            0,
            5,
            245,
            225,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            9,
            105,
            110,
            105,
            116,
            95,
            110,
            97,
            109,
            101,
            0,
            0
        };
        NewInitContract().ToBytes().Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public void ToBytes_InverseOfFromBytes()
    {
        if (InitContract.TryDeserial(NewInitContract().ToBytes(), out var deserial))
        {
            NewInitContract().Should().Be(deserial.InitContract);
        }
        else
        {
            Assert.Fail(deserial.Error);
        }
    }
}
