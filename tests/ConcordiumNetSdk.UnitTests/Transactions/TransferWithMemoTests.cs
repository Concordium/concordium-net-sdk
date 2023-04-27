using ConcordiumNetSdk.Transactions;

using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class SimpleTransferWithMemoPayloadTests
{
    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var ccdAmount = CcdAmount.FromCcd(100);
        var toAccountAddress = AccountAddress.From(
            "3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw"
        );
        var memo = Memo.FromText("message");
        var transferWithMemo = new TransferWithMemo(ccdAmount, toAccountAddress, memo);
        var expectedBytes = new byte[]
        {
            22,
            71,
            16,
            92,
            61,
            132,
            191,
            45,
            174,
            170,
            208,
            206,
            153,
            215,
            123,
            117,
            254,
            225,
            53,
            137,
            184,
            94,
            41,
            112,
            215,
            225,
            165,
            254,
            29,
            145,
            253,
            190,
            160,
            0,
            8,
            103,
            109,
            101,
            115,
            115,
            97,
            103,
            101,
            0,
            0,
            0,
            0,
            5,
            245,
            225,
            0
        };
        transferWithMemo.GetBytes().Should().BeEquivalentTo(expectedBytes);
    }
}
