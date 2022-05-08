using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

// todo: implement tests
/// <summary>
/// Represents a contents of the simple transfer payload that is one of union type of the specific transaction.
/// </summary>
public class SimpleTransferPayload : IAccountTransactionPayload
{
    /// <summary>
    /// Creates an instance of simple transfer payload.
    /// </summary>
    /// <param name="amount">the amount of microCCD that will be sent.</param>
    /// <param name="toAddress">the address of the account to which the transfer will be sent.</param>
    public SimpleTransferPayload(CcdAmount amount, AccountAddress toAddress)
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

    public byte[] SerializeToBytes()
    {
        var result = new List<byte>(41);
        result.Add((byte) AccountTransactionType.SimpleTransfer);
        result.AddRange(ToAddress.AsBytes);
        result.AddRange(Amount.SerializeToBytes());
        return result.ToArray();
    }

    public int GetBaseEnergyCost() => 300;
}
