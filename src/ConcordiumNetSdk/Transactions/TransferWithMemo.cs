using ConcordiumNetSdk.Helpers;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Models a "transfer with memo" account transaction.
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
    private CcdAmount _amount { get; }

    /// <summary>
    /// Address of the receiver account to which the amount will be sent.
    /// </summary>
    private AccountAddress _receiver { get; }

    /// <summary>
    /// Memo to include in the transaction.
    /// </summary>
    private Memo _memo { get; }

    /// <summary>
    /// Get the amount to send.
    /// </summary>
    public CcdAmount GetAmount()
    {
        return CcdAmount.FromMicroCcd(_amount.GetMicroCcdValue());
    }

    /// <summary>
    /// Get the address of the receiver account to which the amount will be sent.
    /// </summary>
    public AccountAddress GetReceiver()
    {
        return AccountAddress.From(_receiver.GetBytes());
    }

    /// <summary>
    /// Get the address of the receiver account to which the amount will be sent.
    /// </summary>
    public Memo GetMemo()
    {
        return Memo.From(_memo.GetBytes());
    }

    /// <summary>
    /// Transaction serialized to the binary format expected by the node.
    /// </summary>
    private byte[] _serializedPayload;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransferWithMemo"/> class.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    /// <param name="memo">Memo to include in the transaction.</param>
    private TransferWithMemo(CcdAmount amount, AccountAddress receiver, Memo memo)
    {
        _amount = amount;
        _receiver = receiver;
        _memo = memo;
        _serializedPayload = Serialize(amount, receiver, memo);
    }

    /// <summary>
    /// Creates an instance of a transfer with memo account transaction.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    /// <param name="memo">Memo to include in the transaction.</param>
    public static AccountTransactionPayload<TransferWithMemo> Create(
        CcdAmount amount,
        AccountAddress receiver,
        Memo memo
    )
    {
        return new TransferWithMemo(amount, receiver, memo);
    }

    /// <summary>
    /// Get the transfer with memo account transaction serialized to the binary format expected by the node.
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
        return (byte[])_serializedPayload.Clone();
    }
}
