using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// An account transaction which is ready to be signed.
/// </summary>
public class PreparedAccountTransaction<T>
    where T : AccountTransactionPayload<T>
{
    /// <summary>
    /// Address of the sender of the transaction.
    /// </summary>
    private AccountAddress _sender;

    /// <summary>
    /// Account nonce to use for the transaction.
    /// </summary>
    private AccountNonce _nonce;

    /// <summary>
    /// Expiration time of the transaction.
    /// </summary>
    private Expiry _expiry;

    /// <summary>
    /// Payload to send to the node.
    /// </summary>
    private AccountTransactionPayload<T> _payload;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreparedAccountTransaction"/> class.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="payload">Payload to send to the node.</param>
    private PreparedAccountTransaction(
        AccountAddress sender,
        AccountNonce nonce,
        Expiry expiry,
        AccountTransactionPayload<T> payload
    )
    {
        this._sender = sender;
        this._nonce = nonce;
        this._expiry = expiry;
        this._payload = payload;
    }

    /// <summary>
    /// Creates a new instance of the prepared transaction.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="payload">Payload to send to the node.</param>
    public static PreparedAccountTransaction<T> Create(
        AccountAddress sender,
        AccountNonce nonce,
        Expiry expiry,
        AccountTransactionPayload<T> payload
    )
    {
        return new PreparedAccountTransaction<T>(sender, nonce, expiry, payload);
    }

    /// <summary>
    /// Signs the prepared transaction using the provided signer.
    /// </summary>
    /// <param name="signer">The signer to use for signing the transaction.</param>
    public SignedAccountTransaction<T> Sign(ITransactionSigner signer)
    {
        return SignedAccountTransaction<T>.Create(_sender, _nonce, _expiry, _payload, signer);
    }
}
