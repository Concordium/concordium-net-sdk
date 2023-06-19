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
    private const byte TransactionType = (byte)AccountTransactionType.SimpleTransferWithMemo;

    /// <summary>
    /// Copies the "transfer with memo" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    /// <param name="memo">Memo to include with the transaction.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver, OnChainData memo)
    {
        using var memoryStream = new MemoryStream((int)(
            sizeof(AccountTransactionType) +
            AccountAddress.BytesLength +
            OnChainData.MaxLength +
            CcdAmount.BytesLength));
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(receiver.ToBytes());
        memoryStream.Write(memo.ToBytes());
        memoryStream.Write(amount.ToBytes());
        return memoryStream.ToArray();
    }

    public override ulong GetTransactionSpecificCost() => 300;

    public override byte[] ToBytes() => Serialize(this.Amount, this.Receiver, this.Memo);
}
