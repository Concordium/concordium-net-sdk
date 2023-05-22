using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents the payload of account transaction.
///
/// Inheriting records should implement data specific to the transaction they
/// model as well as helpers for constructing serialized transaction payloads
/// to be sent to the Concordium node.
/// </summary>
public abstract record AccountTransactionPayload
{
    /// <summary>
    /// Prepares the account transaction payload for signing.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="sequenceNumber">Account sequence number to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    public PreparedAccountTransaction Prepare(
        AccountAddress sender,
        AccountSequenceNumber sequenceNumber,
        Expiry expiry
    ) => new(sender, sequenceNumber, expiry, this);

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
    /// Copies the on-chain data in the binary format expected by the node to a byte array.
    /// </summary>
    public abstract byte[] ToBytes();

    /// <summary>
    /// Converts the transaction to its corresponding protocol buffer message instance.
    /// </summary>
    public Grpc.V2.AccountTransactionPayload ToProto() =>
        new() { RawPayload = Google.Protobuf.ByteString.CopyFrom(this.ToBytes()) };
}
