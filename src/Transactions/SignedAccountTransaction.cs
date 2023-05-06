using Concordium.Grpc.V2;
using System.Security.Cryptography;
using Concordium.Sdk.Types;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a signed account transaction which is ready to be sent to the node.
///
/// A signed account transaction constitutes an <see cref="AccountTransactionHeader"/>, an
/// <see cref="AccountTransactionPayload{T}"/> as well as an <see cref="AccountTransactionSignature"/>.
///
/// A <see cref="SignedAccountTransaction<T>"/> can be created with the
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
        Header = header;
        Payload = payload;
        Signature = signature;
    }

    /// <summary>
    /// Creates a new instance of a signed transaction.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="payload">Payload to send to the node.</param>
    /// <param name="signer">The signer to use for signing the transaction.</param>
    public static SignedAccountTransaction<T> Create(
        AccountAddress sender,
        AccountSequenceNumber nonce,
        Expiry expiry,
        AccountTransactionPayload<T> payload,
        ITransactionSigner transactionSigner
    )
    {
        // Get the serialized payload.
        byte[] serializedPayload = payload.GetBytes();
        UInt32 serializedPayloadSize = (UInt32)serializedPayload.Length;

        // Compute the energy cost.
        UInt64 txSpecificCost = payload.GetTransactionSpecificCost();
        UInt64 energyCost = CalculateEnergyCost(
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
        byte[] serializedHeaderAndTxPayload = header.GetBytes().Concat(serializedPayload).ToArray();
        byte[] signDigest = SHA256.Create().ComputeHash(serializedHeaderAndTxPayload);

        // Sign it.
        AccountTransactionSignature signature = transactionSigner.Sign(signDigest);

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
        UInt32 signatureCount,
        UInt64 txSpecificCost,
        UInt32 headerSize,
        UInt32 payloadSize
    )
    {
        const uint costPerSignature = 100;
        const uint costPerHeaderAndPayloadByte = 1;

        ulong result =
            txSpecificCost
            + costPerSignature * signatureCount
            + costPerHeaderAndPayloadByte * (headerSize + payloadSize);

        return result;
    }

    public AccountTransaction ToProto()
    {
        return new AccountTransaction()
        {
            Header = Header.ToProto(),
            Payload = Payload.ToProto(),
            Signature = Signature.ToProto(),
        };
    }

    /// <summary>
    /// Converts the signed account transaction to a protocol buffer message instance which is compatible with <see cref="SendBlockItem"/>.
    /// </summary>
    public SendBlockItemRequest ToSendBlockItemRequest()
    {
        return new SendBlockItemRequest { AccountTransaction = this.ToProto() };
    }
}
