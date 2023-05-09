using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

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
    public readonly CcdAmount Amount;

    /// <summary>
    /// Address of the receiver account to which the amount will be sent.
    /// </summary>
    public readonly AccountAddress Receiver;

    /// <summary>
    /// Initializes a new instance of the <see cref="Transfer"/> class.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    public Transfer(CcdAmount amount, AccountAddress receiver)
    {
        this.Amount = amount;
        this.Receiver = receiver;
    }

    /// <summary>
    /// Get the "transfer" account transaction serialized to the binary format expected by the node.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver)
    {
        using var memoryStream = new MemoryStream();
        memoryStream.WriteByte(TRANSACTION_TYPE);
        memoryStream.Write(receiver.GetBytes());
        memoryStream.Write(amount.GetBytes());
        return memoryStream.ToArray();
    }

    public override ulong GetTransactionSpecificCost() => 300;

    public override byte[] GetBytes() => Serialize(this.Amount, this.Receiver);
}
