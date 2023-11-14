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

    private const uint BytesLength = sizeof(TransactionType) + AccountAddress.BytesLength + OnChainData.MaxLength + CcdAmount.BytesLength;


    /// <summary>
    /// Copies the "transfer with memo" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    /// <param name="memo">Memo to include with the transaction.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver, OnChainData memo)
    {
        using var memoryStream = new MemoryStream((int)(BytesLength));
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(receiver.ToBytes());
        memoryStream.Write(memo.ToBytes());
        memoryStream.Write(amount.ToBytes());
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Create a "transfer" payload from a serialized as bytes.
    /// </summary>
    /// <param name="bytes">The "transfer" payload as bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(byte[] bytes, out (TransferWithMemo? , String? Error) output) {
        if (bytes.Length != BytesLength) {
            var msg = $"Invalid length in `TransferWithMemo.TryDeserial`. Expected at least {BytesLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        if (bytes[0] != TransactionType) {
			var msg = $"Invalid transaction type in `Transfer.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        };

        var accountLength = (int) AccountAddress.BytesLength;
        var amountLength = (int) CcdAmount.BytesLength;
        var memoLength = (int) 1 - accountLength - amountLength;

        var accountBytes = bytes.Skip(1).Take(accountLength).ToArray();
        var accDeserial = AccountAddress.TryDeserial(accountBytes, out var account);

        if (!accDeserial) {
            output = (null, account.Item2);
            return false;
        };

        var memoBytes = bytes.Skip(1 + accountLength).Take(memoLength).ToArray();
        var memoDeserial = OnChainData.TryDeserial(memoBytes, out var memo);

        if (!memoDeserial) {
            output = (null, memo.Item2);
            return false;
        };

        var amountBytes = bytes.Skip(1 + accountLength + memoLength).Take(amountLength).ToArray();
        var amountDeserial = CcdAmount.TryDeserial(amountBytes, out var amount);

        if (!amountDeserial) {
            output = (null, amount.Item2);
            return false;
        };

        output = (new TransferWithMemo(amount.Item1.Value, account.Item1, memo.Item1), null);
        return false;
    }

    public override ulong GetTransactionSpecificCost() => 300;

    public override byte[] ToBytes() => Serialize(this.Amount, this.Receiver, this.Memo);
}
