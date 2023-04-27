using ConcordiumNetSdk.Helpers;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents a "transfer with memo" account transaction.
///
/// Used for transferring CCD from one account to another. Like <see cref="Transfer"/>,
/// but additionally stores an on-chain <see cref="Memo"/> with the transfer.
/// </summary>
public record TransferWithMemo : AccountTransactionPayload<TransferWithMemo>
{
    /// <summary>
    /// The account transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TRANSACTION_TYPE = (byte)AccountTransactionType.SimpleTransferWithMemo;

    /// <summary>
    /// Amount to send.
    /// </summary>
    public readonly CcdAmount Amount;

    /// <summary>
    /// Address of the receiver account to which the amount will be sent.
    /// </summary>
    public readonly AccountAddress Receiver;

    /// <summary>
    /// Memo to include in the transaction.
    /// </summary>
    public readonly Memo Memo;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransferWithMemo"/> class.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    /// <param name="memo">Memo to include in the transaction.</param>
    public TransferWithMemo(CcdAmount amount, AccountAddress receiver, Memo memo)
    {
        Amount = amount;
        Receiver = receiver;
        Memo = memo;
    }

    /// <summary>
    /// Get the "transfer with memo" account transaction serialized to the binary format expected by the node.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    /// <param name="memo">Memo to include in the transaction.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver, Memo memo)
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.WriteByte(TRANSACTION_TYPE);
        memoryStream.Write(receiver.GetBytes());
        memoryStream.Write(memo.GetBytes());
        memoryStream.Write(Serialization.GetBytes(amount.GetMicroCcdValue()));
        return memoryStream.ToArray();
    }

    public override ulong GetBaseEnergyCost() => 300;

    public override byte[] GetBytes()
    {
        return Serialize(Amount, Receiver, Memo);
    }
}
