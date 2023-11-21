using System.Security.Cryptography;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents an account transaction which is prepared for signing.
///
/// The transaction is prepared in the sense that it is augmented with the
/// <see cref="AccountAddress"/> of the sender, the <see cref="AccountSequenceNumber"/> to use
/// when submitting the transaction as well as its <see cref="Types.Expiry"/>. These are also
/// used when signing the transaction.
/// </summary>
/// <param name="Sender">Address of the sender of the transaction.</param>
/// <param name="SequenceNumber">Account sequence number to use for the transaction.</param>
/// <param name="Expiry">Expiration time of the transaction.</param>
/// <param name="Energy">The maximum energy cost of this transaction.</param>
/// <param name="Payload">Payload to send to the node.</param>
public sealed record PreparedAccountTransaction(
    AccountAddress Sender,
    AccountSequenceNumber SequenceNumber,
    Expiry Expiry,
    EnergyAmount Energy,
    AccountTransactionPayload Payload
    )
{
    /// <summary>
    /// Signs the prepared transaction using the provided signer.
    /// </summary>
    /// <param name="transactionSigner">The signer to use for signing the transaction.</param>
    /// <exception cref="ArgumentException">A signature produced by the signing is not 64 bytes.</exception>
    public SignedAccountTransaction Sign(ITransactionSigner transactionSigner)
    {
        // Get the serialized payload.
        var serializedPayload = this.Payload.ToBytes();
        var serializedPayloadSize = (uint)serializedPayload.Length;

        // Compute the energy cost.
        var energyCost = CalculateEnergyCost(
            transactionSigner.GetSignatureCount(),
            this.Energy.Value,
            AccountTransactionHeader.BytesLength,
            serializedPayloadSize
        );

        // Create the header.
        var header = new AccountTransactionHeader(
            this.Sender,
            this.SequenceNumber,
            this.Expiry,
            energyCost,
            new PayloadSize(serializedPayloadSize)
        );

        // Construct the serialized payload and its digest for signing.
        var serializedHeaderAndTxPayload = header.ToBytes().Concat(serializedPayload).ToArray();

        using var hasher = SHA256.Create();
        var signDigest = hasher.ComputeHash(serializedHeaderAndTxPayload);

        // Sign it.
        var signature = transactionSigner.Sign(signDigest);

        return new(header, this.Payload, signature);
    }

    /// <summary>
    /// Calculates the energy cost associated with processing the transaction.
    /// </summary>
    /// <param name="signatureCount">The number of signatures.</param>
    /// <param name="txSpecificCost">The transaction specific cost.</param>
    /// <param name="headerSize">The size of the header in bytes.</param>
    /// <param name="payloadSize">The size of the payload in bytes.</param>
    private static EnergyAmount CalculateEnergyCost(
        uint signatureCount,
        ulong txSpecificCost,
        uint headerSize,
        uint payloadSize
    )
    {
        const uint costPerSignature = 100;
        const uint costPerHeaderAndPayloadByte = 1;

        var result =
            txSpecificCost
            + (costPerSignature * signatureCount)
            + (costPerHeaderAndPayloadByte * (headerSize + payloadSize));

        return new(result);
    }
}
