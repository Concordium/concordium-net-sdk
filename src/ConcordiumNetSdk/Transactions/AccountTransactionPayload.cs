using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents the payload of account transaction.
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
        AccountSequenceNumber nonce,
        Expiry expiry
    )
    {
        return new PreparedAccountTransaction<T>(sender, nonce, expiry, this);
    }

    /// <summary>
    /// Gets the transaction specific cost for submitting this type of
    /// transaction to the chain.
    ///
    /// This should reflect the transaction-specific costs defined here:
    /// https://github.com/Concordium/concordium-base/blob/78f557b8b8c94773a25e4f86a1a92bc323ea2e3d/haskell-src/Concordium/Cost.hs
    ///
    /// Note that this is only part of the cost of a transaction, and
    /// does not include costs associated with verification of signatures
    /// as well as costs that are incurred at execution time, for instance
    /// when initializing or updating a smart contract.
    /// </summary>
    public abstract ulong GetTransactionSpecificCost();

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
