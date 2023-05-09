using Concordium.Grpc.V2;

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
    public AccountTransactionHeader Header { get; init; }

    /// <summary>
    /// Payload of the signed account transaction.
    /// </summary>
    public AccountTransactionPayload<T> Payload { get; init; }

    /// <summary>
    /// Signature of the signed account transaction.
    /// </summary>
    public AccountTransactionSignature Signature { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignedAccountTransaction{T}"/> class.
    /// </summary>
    /// <param name="data">Header of the signed account transaction.</param>
    /// <param name="payload">Payload of the signed account transaction.</param>
    /// <param name="signature">Signature of the signed account transaction.</param>
    public SignedAccountTransaction(
        AccountTransactionHeader header,
        AccountTransactionPayload<T> payload,
        AccountTransactionSignature signature
    )
    {
        this.Header = header;
        this.Payload = payload;
        this.Signature = signature;
    }

    public AccountTransaction ToProto() =>
        new()
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
    public SendBlockItemRequest ToSendBlockItemRequest() =>
        new() { AccountTransaction = this.ToProto() };
}
