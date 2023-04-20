using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Account transaction payload.
/// </summary>
public abstract class AccountTransactionPayload<T>
    where T : AccountTransactionPayload<T>
{
    /// <summary>
    /// Prepares the account transaction payload for signing.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="nonce">Account nonce to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    public PreparedAccountTransaction<T> Prepare(
        AccountAddress sender,
        AccountNonce nonce,
        Expiry expiry
    )
    {
        return PreparedAccountTransaction<T>.Create(sender, nonce, expiry, this);
    }

    /// <summary>
    /// Gets the base cost for submitting the transaction payload.
    /// </summary>
    public abstract ulong GetBaseEnergyCost();

    /// <summary>
    /// Gets the transaction payload in the binary format expected by the node.
    /// </summary>
    public abstract byte[] GetBytes();

    /// <summary>
    /// Converts the transaction to its corresponding protocol buffer message instance.
    /// </summary>
    public abstract Concordium.V2.AccountTransactionPayload ToProto();
}
