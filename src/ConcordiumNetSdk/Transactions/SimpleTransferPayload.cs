using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents a contents of the simple transfer payload that is one of union type of the specific transaction.
/// </summary>
public class SimpleTransferPayload : IAccountTransactionPayload
{
    private SimpleTransferPayload(CcdAmount amount, AccountAddress toAddress)
    {
        Amount = amount;
        ToAddress = toAddress;
    }

    /// <summary>
    /// Gets the amount of microCCD that will be sent.
    /// </summary>
    public CcdAmount Amount { get; }

    /// <summary>
    /// Gets the address of the account to which the transfer will be sent.
    /// </summary>
    public AccountAddress ToAddress { get; }

    /// <summary>
    /// Creates an instance of simple transfer payload.
    /// </summary>
    /// <param name="amount">the amount of microCCD that will be sent.</param>
    /// <param name="toAddress">the address of the account to which the transfer will be sent.</param>
    public static SimpleTransferPayload Create(CcdAmount amount, AccountAddress toAddress)
    {
        return new SimpleTransferPayload(amount, toAddress);
    }

    public byte[] SerializeToBytes()
    {
        byte[] result = new byte[41];
        Span<byte> span = result;
        result[0] = (byte) AccountTransactionType.SimpleTransfer;
        ToAddress.AsBytes.CopyTo(span.Slice(1, 32));
        Amount.SerializeToBytes().CopyTo(span.Slice(33));
        return result;
    }

    public ulong GetBaseEnergyCost() => 300;
}
