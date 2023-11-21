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
/// <param name="Header">Header of the signed account transaction.</param>
/// <param name="Payload">Payload of the signed account transaction.</param>
/// <param name="Signature">Signature of the signed account transaction.</param>
public record SignedAccountTransaction(
    AccountTransactionHeader Header,
    AccountTransactionPayload Payload,
    AccountTransactionSignature Signature
) : BlockItemType
{
    /// <summary>Converts this type to the equivalent protocol buffer type.</summary>
    public AccountTransaction ToProto() =>
        new()
        {
            Header = this.Header.ToProto(),
            Payload = this.Payload.ToProto(),
            Signature = this.Signature.ToProto(),
        };

    internal static SignedAccountTransaction From(AccountTransaction accountTransaction) => new(
            AccountTransactionHeader.From(accountTransaction.Header),
            AccountTransactionPayload.From(accountTransaction.Payload),
            AccountTransactionSignature.From(accountTransaction.Signature)
        );

    /// <summary>
    /// Converts the signed account transaction to a protocol buffer
    /// message instance which is compatible with
    /// <see cref="Client.RawClient.SendBlockItem"/>.
    /// </summary>
    public SendBlockItemRequest ToSendBlockItemRequest() =>
        new() { AccountTransaction = this.ToProto() };
}
