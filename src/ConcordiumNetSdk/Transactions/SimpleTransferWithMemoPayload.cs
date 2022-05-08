using System.Buffers.Binary;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

// todo: implement tests
/// <summary>
/// Represents a contents of the simple transfer with memo payload that is one of union type of the specific transaction.
/// </summary>
public class SimpleTransferWithMemoPayload : IAccountTransactionPayload
{
    /// <summary>
    /// Creates an instance of simple transfer with memo payload.
    /// </summary>
    /// <param name="amount">the amount of microCCD that will be sent.</param>
    /// <param name="toAddress">the address of the account to which the transfer will be sent.</param>
    /// <param name="memo">the memo.</param>
    public SimpleTransferWithMemoPayload(CcdAmount amount, AccountAddress toAddress, Memo memo)
    {
        Amount = amount;
        ToAddress = toAddress ?? throw new ArgumentNullException(nameof(toAddress));
        Memo = memo ?? throw new ArgumentNullException(nameof(memo));
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
    /// Gets the memo that will be sent.
    /// </summary>
    public Memo Memo { get; }

    public byte[] SerializeToBytes()
    {
        var memoLength = Memo.AsBytes.Length;
        var serializedLength = 43 + memoLength;
        var result = new byte[serializedLength];

        Span<byte> buffer = result;
        buffer[0] = (byte) AccountTransactionType.SimpleTransferWithMemo;
        ToAddress.AsBytes.CopyTo(buffer.Slice(1, 32));
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(33, 2), Convert.ToUInt16(memoLength));
        Memo.AsBytes.CopyTo(buffer.Slice(35, memoLength));
        Amount.SerializeToBytes().CopyTo(buffer.Slice(35 + memoLength, 8));

        return result;
    }

    public int GetBaseEnergyCost()
    {
        return 300;
    }
}
