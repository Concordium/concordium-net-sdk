using Concordium.Grpc.V2;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a signed account transaction which is ready to be sent to the node.
///
/// A signed account transaction constitutes an <see cref="AccountTransactionHeader"/>, an
/// <see cref="AccountTransactionPayload"/> as well as an <see cref="AccountTransactionSignature"/>.
///
/// A <see cref="SignedAccountTransaction"/> can be created with the
/// <see cref="PreparedAccountTransaction.Sign"/> method with an implementer of
/// <see cref="ITransactionSigner"/>.
/// </summary>
public record SignedAccountTransaction
{
    /// <summary>
    /// Header of the signed account transaction.
    /// </summary>
    public AccountTransactionHeader Header { get; init; }

    /// <summary>
    /// Payload of the signed account transaction.
    /// </summary>
    public AccountTransactionPayload Payload { get; init; }

    /// <summary>
    /// Signature of the signed account transaction.
    /// </summary>
    public AccountTransactionSignature Signature { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignedAccountTransaction"/> class.
    /// </summary>
    /// <param name="data">Header of the signed account transaction.</param>
    /// <param name="payload">Payload of the signed account transaction.</param>
    /// <param name="signature">Signature of the signed account transaction.</param>
    public SignedAccountTransaction(
        AccountTransactionHeader header,
        AccountTransactionPayload payload,
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
