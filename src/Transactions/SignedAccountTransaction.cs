using System.Security.Cryptography;
using Concordium.Grpc.V2;
using Concordium.Sdk.Types;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a signed account transaction which is ready to be sent to the node.
///
/// A signed account transaction constitutes an <see cref="AccountTransactionHeader"/>, an
/// <see cref="AccountTransactionPayload{T}"/> as well as an <see cref="AccountTransactionSignature"/>.
///
/// A <see cref="SignedAccountTransaction{T}"/> can be created with the
/// <see cref="PreparedAccountTransaction{T}.Sign"/> method with an implementer of
/// <see cref="ITransactionSigner"/>.
/// </summary>
public record SignedAccountTransaction<T>
    where T : AccountTransactionPayload<T>
{
    /// <summary>
    /// Header of the signed account transaction.
    /// </summary>
    public readonly AccountTransactionHeader Header;

    /// <summary>
    /// Payload of the signed account transaction.
    /// </summary>
    public readonly AccountTransactionPayload<T> Payload;

    /// <summary>
    /// Signature of the signed account transaction.
    /// </summary>
    public readonly AccountTransactionSignature Signature;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignedAccountTransaction"/> class.
    /// </summary>
    /// <param name="data">Header of the signed account transaction.</param>
    /// <param name="payload">Payload of the signed account transaction.</param>
    /// <param name="signature">Signature of the signed account transaction.</param>
    private SignedAccountTransaction(
        AccountTransactionHeader header,
        AccountTransactionPayload<T> payload,
        AccountTransactionSignature signature
    )
    {
        this.Header = header;
        this.Payload = payload;
        this.Signature = signature;
    }

    /// <summary>
    /// Creates a new instance of a signed transaction.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="payload">Payload to send to the node.</param>
    /// <param name="transactionSigner">The signer to use for signing the transaction.</param>
    public static SignedAccountTransaction<T> Create(
        AccountAddress sender,
        AccountSequenceNumber nonce,
        Expiry expiry,
        AccountTransactionPayload<T> payload,
        ITransactionSigner transactionSigner
    )
    {
        // Get the serialized payload.
        var serializedPayload = payload.GetBytes();
        var serializedPayloadSize = (uint)serializedPayload.Length;

        // Compute the energy cost.
        var txSpecificCost = payload.GetTransactionSpecificCost();
        var energyCost = CalculateEnergyCost(
            transactionSigner.GetSignatureCount(),
            txSpecificCost,
            AccountTransactionHeader.BytesLength,
            serializedPayloadSize
        );

        // Create the header.
        var header = new AccountTransactionHeader(
            sender,
            nonce,
            expiry,
            energyCost,
            serializedPayloadSize
        );

        // Construct the serialized payload and its digest for signing.
        var serializedHeaderAndTxPayload = header.GetBytes().Concat(serializedPayload).ToArray();
        var signDigest = SHA256.Create().ComputeHash(serializedHeaderAndTxPayload);

        // Sign it.
        var signature = transactionSigner.Sign(signDigest);

        return new SignedAccountTransaction<T>(header, payload, signature);
    }

    /// <summary>
    /// Calculates the energy cost associated with processing the transaction.
    /// </summary>
    /// <param name="signatureCount">The number of signatures.</param>
    /// <param name="txSpecificCost">The transaction specific cost.</param>
    /// <param name="headerSize">The size of the header in bytes.</param>
    /// <param name="payloadSize">The size of the payload in bytes.</param>
    public static ulong CalculateEnergyCost(
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

        return result;
    }

    public AccountTransaction ToProto() => new()
    {
        Header = this.Header.ToProto(),
        Payload = this.Payload.ToProto(),
        Signature = this.Signature.ToProto(),
    };

    /// <summary>
    /// Converts the signed account transaction to a protocol buffer
    /// message instance which is compatible with
    /// <see cref="Client.RawClient.SendBlockItem"/>.
    /// </summary>
    public SendBlockItemRequest ToSendBlockItemRequest() => new() { AccountTransaction = this.ToProto() };
}
