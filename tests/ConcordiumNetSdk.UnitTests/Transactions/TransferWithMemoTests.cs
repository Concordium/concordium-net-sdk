using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class SimpleTransferWithMemoPayloadTests
{
    public static TransferWithMemo CreateTransferWithMemo()
    {
        var amount = CcdAmount.FromCcd(100);
        var receiver = AccountAddress.From("3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw");
        var memo = Memo.FromText("message");
        return new TransferWithMemo(amount, receiver, memo);
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
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
        CreateTransferWithMemo().GetBytes().Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public void Prepare_ThenSign_HasCorrectSignatures()
    {
        // Create the transfer.
        TransferWithMemo transfer = CreateTransferWithMemo();

        // Prepare the transfer.
        PreparedAccountTransaction<TransferWithMemo> preparedTransfer =
            TransactionTestHelpers<TransferWithMemo>.CreatePreparedAccountTransaction(transfer);

        // Sign the transfer.
        SignedAccountTransaction<TransferWithMemo> signedTransfer =
            TransactionTestHelpers<TransferWithMemo>.CreateSignedTransaction(preparedTransfer);

        // The expected signature.
        byte[] expectedSignature00 = Convert.FromHexString(
            "29aa3584ede6335ab34fac2b0fc2f788087d01043f636eab9be5e682448bdaf0cabdcef2d1978c15116dd7e363eb383aa3176fb4881890fcc1e0d72782a01e03"
        );
        byte[] expectedSignature01 = Convert.FromHexString(
            "d6fe41133848aca9821e0cb07f1a236d124df35a9f8113e7ca7017bd8242f3b53ad997b0cdcf039baa20e1765a05311a676f947093239d98f15a38da98001904"
        );
        byte[] expectedSignature11 = Convert.FromHexString(
            "ab0b927572244f85e45cef55757dc27810f2aa041b0298e3acad26df5eabf09192b9c2fcfb0f5dbfcee4b5a376de5a5b1f86209ead70917fa66e3bd84c9a5a07"
        );

        AccountTransactionSignature expectedSignature = new AccountTransactionSignature(
            new Dictionary<AccountCredentialIndex, Dictionary<AccountKeyIndex, byte[]>>()
            {
                // Credential index 0.
                {
                    0,
                    new Dictionary<AccountKeyIndex, byte[]>()
                    {
                        // Key index 0.
                        { 0, expectedSignature00 },
                        // Key index 1.
                        { 1, expectedSignature01 }
                    }
                },
                // Credential index 1.
                {
                    1,
                    new Dictionary<AccountKeyIndex, byte[]>()
                    {
                        // Key index 1.
                        { 1, expectedSignature11 }
                    }
                },
            }
        );

        signedTransfer.Signature.signatureMap
            .Should()
            .BeEquivalentTo(expectedSignature.signatureMap);
    }
}
