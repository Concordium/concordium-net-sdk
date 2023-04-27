using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Models the payload of account transaction.
///
/// Inheriting records should implement data specific to the transaction they
/// model as well as helpers for constructing serialized transaction payloads
/// to be sent to the Concordium node.
/// </summary>
public abstract record AccountTransactionPayload<T>
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
        return new PreparedAccountTransaction<T>(sender, nonce, expiry, this);
    }

    /// <summary>
    /// Get the base cost for submitting the transaction payload.
    /// </summary>
    public abstract ulong GetBaseEnergyCost();

    /// <summary>
    /// Get the transaction payload in the binary format expected by the node.
    /// </summary>
    public abstract byte[] GetBytes();

    /// <summary>
    /// Converts the transaction to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.AccountTransactionPayload ToProto()
    {
        return new Concordium.V2.AccountTransactionPayload()
        {
            RawPayload = Google.Protobuf.ByteString.CopyFrom(GetBytes())
        };
    }
}
