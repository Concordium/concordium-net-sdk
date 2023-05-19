using System;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Transactions;

public class SimpleTransferWithMemoPayloadTests
{
    /// <summary>
    /// Creates a new instance of the <see cref="TransferWithMemo"/>
    /// transaction of <c>100</c> CCD and with
    /// <c>"3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw"</c>
    /// as the receiver address and <c>"message"</c> encoded as CBOR
    /// using the strict conformance mode as the memo.
    /// </summary>
    public static TransferWithMemo CreateTransferWithMemo()
    {
        var amount = CcdAmount.FromCcd(100);
        var receiver = AccountAddress.From("3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw");
        var memo = OnChainData.FromTextEncodeAsCBOR("message");
        return new TransferWithMemo(amount, receiver, memo);
    }

    [Fact]
    public void ToBytes_ReturnsCorrectValue()
    {
        // The expected payload was generated using the Concordium Rust SDK.
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
        CreateTransferWithMemo().ToBytes().Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public void Prepare_ThenSign_ProducesCorrectSignatures()
    {
        // Create the transfer.
        var transfer = CreateTransferWithMemo();

        // Prepare the transfer.
        var preparedTransfer =
            TransactionTestHelpers.CreatePreparedAccountTransaction(transfer);

        // Sign the transfer.
        var signedTransfer =
            TransactionTestHelpers.CreateSignedTransaction(preparedTransfer);

        // Create the expected signature. It was generated using the corresponding method in the Concordium Rust SDK.
        var expectedSignature00 = Convert.FromHexString(
            "29aa3584ede6335ab34fac2b0fc2f788087d01043f636eab9be5e682448bdaf0cabdcef2d1978c15116dd7e363eb383aa3176fb4881890fcc1e0d72782a01e03"
        );
        var expectedSignature01 = Convert.FromHexString(
            "d6fe41133848aca9821e0cb07f1a236d124df35a9f8113e7ca7017bd8242f3b53ad997b0cdcf039baa20e1765a05311a676f947093239d98f15a38da98001904"
        );
        var expectedSignature11 = Convert.FromHexString(
            "ab0b927572244f85e45cef55757dc27810f2aa041b0298e3acad26df5eabf09192b9c2fcfb0f5dbfcee4b5a376de5a5b1f86209ead70917fa66e3bd84c9a5a07"
        );

        var expectedSignature = TransactionTestHelpers.FromExpectedSignatures(
            expectedSignature00,
            expectedSignature01,
            expectedSignature11
        );

        signedTransfer.Signature.SignatureMap
            .Should()
            .BeEquivalentTo(expectedSignature.SignatureMap);
    }
}
