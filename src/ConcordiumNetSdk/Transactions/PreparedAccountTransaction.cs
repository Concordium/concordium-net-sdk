using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents an account transaction which is ready to be signed.
///
/// A prepared account transaction is <see cref="T"/>
/// </summary>
public record PreparedAccountTransaction<T>
    where T : AccountTransactionPayload<T>
{
    /// <summary>
    /// Address of the sender of the transaction.
    /// </summary>
    public readonly AccountAddress Sender;

    /// <summary>
    /// Account nonce to use for the transaction.
    /// </summary>
    public readonly AccountNonce Nonce;

    /// <summary>
    /// Expiration time of the transaction.
    /// </summary>
    public readonly Expiry Expiry;

    /// <summary>
    /// Payload to send to the node.
    /// </summary>
    public readonly AccountTransactionPayload<T> Payload;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreparedAccountTransaction"/> class.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="payload">Payload to send to the node.</param>
    public PreparedAccountTransaction(
        AccountAddress sender,
        AccountNonce nonce,
        Expiry expiry,
        AccountTransactionPayload<T> payload
    )
    {
        Sender = sender;
        Nonce = nonce;
        Expiry = expiry;
        Payload = payload;
    }

    /// <summary>
    /// Signs the prepared transaction using the provided signer.
    /// </summary>
    /// <param name="signer">The signer to use for signing the transaction.</param>
    public SignedAccountTransaction<T> Sign(ITransactionSigner signer)
    {
        return SignedAccountTransaction<T>.Create(Sender, Nonce, Expiry, Payload, signer);
    }
}
