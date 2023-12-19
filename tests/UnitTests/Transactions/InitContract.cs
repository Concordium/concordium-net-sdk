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
    public static InitContract newInitContract()
    {
        var amount = CcdAmount.FromCcd(100);
		var moduleRef = new ModuleReference("0000000000000000000000000000000000000000000000000000000000000000");
		var initName = new InitName("init_name");
		var parameter = new Parameter(new byte[0]);

        return new InitContract(amount, moduleRef, initName, parameter);
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
        newInitContract().ToBytes().Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public void ToBytes_InverseOfFromBytes()
    {
        if (InitContract.TryDeserial(newInitContract().ToBytes(), out var deserial))
        {
            newInitContract().Should().Be(deserial.InitContract);
        }
        else
        {
            Assert.Fail(deserial.Error);
        }
    }
}
