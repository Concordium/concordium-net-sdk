using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a "transfer with memo" account transaction.
///
/// Used for transferring CCD from one account to another. Like <see cref="Transfer"/>,
/// but additionally stores a memo associated with the transfer on-chain.
/// </summary>
/// <param name="Amount">Amount to send.</param>
/// <param name="Receiver">Address of the receiver account to which the amount will be sent.</param>
/// <param name="Memo">Memo to include with the transaction.</param>
public sealed record TransferWithMemo(CcdAmount Amount, AccountAddress Receiver, OnChainData Memo) : AccountTransactionPayload
{
    /// <summary>
    /// The account transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TransactionType = (byte)Types.TransactionType.TransferWithMemo;

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
    /// Copies the "transfer with memo" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    /// <param name="memo">Memo to include with the transaction.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver, OnChainData memo)
    {
        using var memoryStream = new MemoryStream((int)(
            sizeof(TransactionType) +
            CcdAmount.BytesLength +
            AccountAddress.BytesLength +
            OnChainData.MaxLength));
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(receiver.ToBytes());
        memoryStream.Write(memo.ToBytes());
        memoryStream.Write(amount.ToBytes());
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Create a "transfer with memo" payload from a serialized as bytes.
    /// </summary>
    /// <param name="bytes">The payload as bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(byte[] bytes, out (TransferWithMemo?, string? Error) output)
    {
        var minSize = sizeof(TransactionType) + AccountAddress.BytesLength + CcdAmount.BytesLength;
        if (bytes.Length < minSize)
        {
            var msg = $"Invalid length in `TransferWithMemo.TryDeserial`. Expected at least {minSize}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        if (bytes[0] != TransactionType)
        {
            var msg = $"Invalid transaction type in `Transfer.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        };

        var trxTypeLength = sizeof(TransactionType);
        var accountLength = (int)AccountAddress.BytesLength;
        var amountLength = (int)CcdAmount.BytesLength;
        var memoLength = bytes.Length - trxTypeLength - accountLength - amountLength;

        var accountBytes = bytes.Skip(trxTypeLength).Take(accountLength).ToArray();
        var accDeserial = AccountAddress.TryDeserial(accountBytes, out var account);

        if (!accDeserial)
        {
            output = (null, account.Error);
            return false;
        };

        var memoBytes = bytes.Skip(trxTypeLength + accountLength).Take(memoLength).ToArray();
        var memoDeserial = OnChainData.TryDeserial(memoBytes, out var memo);

        if (!memoDeserial)
        {
            output = (null, memo.Error);
            return false;
        };

        var amountBytes = bytes.Skip(trxTypeLength + accountLength + memoLength).Take(amountLength).ToArray();
        var amountDeserial = CcdAmount.TryDeserial(amountBytes, out var amount);

        if (!amountDeserial)
        {
            output = (null, amount.Error);
            return false;
        };

        output = (new TransferWithMemo(amount.accountAddress.Value, account.accountAddress, memo.accountAddress), null);
        return true;
    }

    public override byte[] ToBytes() => Serialize(this.Amount, this.Receiver, this.Memo);
}
