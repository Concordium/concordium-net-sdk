using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class TransferTests
{
    /// <summary>
    /// Creates a new instance of the <see cref="Transfer"/>
    /// transaction of <c>100</c> CCD and with
    /// <c>3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw</c>
    /// as the receiver address.
    /// </summary>
    public static Transfer CreateTransfer()
    {
        var amount = CcdAmount.FromCcd(100);
        var receiver = AccountAddress.From("3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw");
        return new Transfer(amount, receiver);
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var expectedBytes = new byte[]
        {
            3,
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
            0,
            0,
            0,
            5,
            245,
            225,
            0
        };
        CreateTransfer().GetBytes().Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public void Prepare_ThenSign_ProducesCorrectSignatures()
    {
        // Create the transfer.
        Transfer transfer = CreateTransfer();

        // Prepare the transfer.
        PreparedAccountTransaction<Transfer> preparedTransfer =
            TransactionTestHelpers<Transfer>.CreatePreparedAccountTransaction(transfer);

        // Sign the transfer.
        SignedAccountTransaction<Transfer> signedTransfer =
            TransactionTestHelpers<Transfer>.CreateSignedTransaction(preparedTransfer);

        // Create the expected signature.
        byte[] expectedSignature00 = Convert.FromHexString(
            "339222503ba5c5a7365612d3bcb3e913fe99666182d6f46648ed22bc89e50178d77d9a858d320a1730b965db7a90c54dbddd801c857b3c21b9c67a73abcf8c09"
        );
        byte[] expectedSignature01 = Convert.FromHexString(
            "fea460f2e152b430203970690a463654f2d6e28664b01dec86580194ecb9f703d8c8664e3fa71b94c8612885d8ab166828c88c2c1e6dcbe036d12aae7e37bc0b"
        );
        byte[] expectedSignature11 = Convert.FromHexString(
            "535c8c43e355c7f83c4e2905f2f489c38ef0d9f92dcd164ee92d08439af9dcb8f7cfae3103b24016db40437dcdfe964e3fe23ea786c863ccb3da30fe6a3dd50c"
        );

        var expectedSignature = TransactionTestHelpers<RegisterData>.FromExpectedSignatures(
            expectedSignature00,
            expectedSignature01,
            expectedSignature11
        );

        signedTransfer.Signature.signatureMap
            .Should()
            .BeEquivalentTo(expectedSignature.signatureMap);
    }
}
