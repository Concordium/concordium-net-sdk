using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a "register data" account transaction.
///
/// Used for registering data on-chain.
/// </summary>
/// <param name="Data">The data to be registered on-chain.</param>
public sealed record RegisterData(OnChainData Data) : AccountTransactionPayload
{
    /// <summary>
    /// The account transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TransactionType = (byte)Types.TransactionType.RegisterData;

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
    ) => new(sender, sequenceNumber, expiry, this._transactionCost, this);

    /// <summary>
    /// The transaction specific cost for submitting this type of
    /// transaction to the chain.
    ///
    /// This should reflect the transaction-specific costs defined here:
    /// https://github.com/Concordium/concordium-base/blob/78f557b8b8c94773a25e4f86a1a92bc323ea2e3d/haskell-src/Concordium/Cost.hs
    /// </summary>
    private readonly EnergyAmount _transactionCost = new(300);

    /// <summary>
    /// Gets the size (number of bytes) of the payload.
    /// </summary>
    internal override PayloadSize Size() => new(sizeof(TransactionType) + this.Data.SerializedLength());

    /// <summary>
    /// Create a "register data" payload from a serialized as bytes.
    /// </summary>
    /// <param name="bytes">The payload as bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (RegisterData? RegisterData, string? Error) output)
    {
        var minSize = sizeof(TransactionType);
        if (bytes.Length < minSize)
        {
            var msg = $"Invalid length in `TransferWithMemo.TryDeserial`. Expected at least {minSize}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        if (bytes[0] != TransactionType)
        {
            var msg = $"Invalid transaction type in `Transfer.TryDeserial`. Expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        };

        var memoBytes = bytes[sizeof(TransactionType)..];
        if (!OnChainData.TryDeserial(memoBytes, out var memo))
        {
            output = (null, memo.Error);
            return false;
        };

        if (memo.OnChainData == null)
        {
            throw new DeserialNullException();
        };

        output = (new RegisterData(memo.OnChainData), null);
        return true;
    }

    /// <summary>
    /// Copies the "register data" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    public override byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.Size().Size);
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(this.Data.ToBytes());
        return memoryStream.ToArray();
    }

}
