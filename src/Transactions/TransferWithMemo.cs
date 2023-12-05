using System.Buffers.Binary;
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
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (TransferWithMemo? Transfer, string? Error) output)
    {
        var minSize = sizeof(TransactionType) + AccountAddress.BytesLength + CcdAmount.BytesLength + sizeof(ushort);
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
        var memoLength = BinaryPrimitives.ReadUInt16BigEndian(bytes[(trxTypeLength + accountLength)..]) + sizeof(ushort);

        var accountBytes = bytes[trxTypeLength..];
        if (!AccountAddress.TryDeserial(accountBytes, out var account))
        {
            output = (null, account.Error);
            return false;
        };

        var memoBytes = bytes[(trxTypeLength + accountLength)..];
        if (!OnChainData.TryDeserial(memoBytes, out var memo))
        {
            output = (null, memo.Error);
            return false;
        };

        var amountBytes = bytes[(trxTypeLength + accountLength + memoLength)..];
        if (!CcdAmount.TryDeserial(amountBytes, out var amount))
        {
            output = (null, amount.Error);
            return false;
        };

        if (amount.Amount == null || account.AccountAddress == null || memo.OnChainData == null)
        {
            var msg = $"The parsed output is null, but no error was found. This should not be possible.";
            output = (null, msg);
            return false;
        };

        output = (new TransferWithMemo(amount.Amount.Value, account.AccountAddress, memo.OnChainData), null);
        return true;
    }

    public override byte[] ToBytes() => Serialize(this.Amount, this.Receiver, this.Memo);
}
