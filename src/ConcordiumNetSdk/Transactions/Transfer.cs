using System.Buffers.Binary;
using ConcordiumNetSdk.Helpers;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents a "transfer" account transaction.
///
/// Used for transferring CCD from one account to another.
/// </summary>
public record Transfer : AccountTransactionPayload<Transfer>
{
    /// <summary>
    /// The account transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TRANSACTION_TYPE = (byte)AccountTransactionType.SimpleTransfer;

    /// <summary>
    /// Amount to send.
    /// </summary>
    private CcdAmount _amount;

    /// <summary>
    /// Address of the receiver account to which the amount will be sent.
    /// </summary>
    private AccountAddress _receiver;

    /// <summary>
    /// Transaction serialized to the binary format expected by the node.
    /// </summary>
    private byte[] _serializedPayload;

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
    /// Initializes a new instance of the <see cref="Transfer"/> class.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    private Transfer(CcdAmount amount, AccountAddress receiver)
    {
        _amount = amount;
        _receiver = receiver;
        _serializedPayload = Serialize(amount, receiver);
    }

    /// <summary>
    /// Creates an instance of transfer account transction.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    public static AccountTransactionPayload<Transfer> Create(
        CcdAmount amount,
        AccountAddress receiver
    )
    {
        return new Transfer(amount, receiver);
    }

    /// <summary>
    /// Get the transfer account transaction serialized to the binary format expected by the node.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver)
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.WriteByte(TRANSACTION_TYPE);
        memoryStream.Write(receiver.GetBytes());
        memoryStream.Write(Serialization.GetBytes(amount.GetMicroCcdValue()));
        return memoryStream.ToArray();
    }

    public override ulong GetBaseEnergyCost() => 300;

    public override byte[] GetBytes()
    {
        return (byte[])_serializedPayload.Clone();
    }
}
