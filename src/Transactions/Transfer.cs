using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a "transfer" account transaction.
///
/// Used for transferring CCD from one account to another.
/// </summary>
/// <param name="Amount">Amount to send.</param>
/// <param name="Receiver">Address of the receiver account to which the amount will be sent.</param>
public sealed record Transfer(CcdAmount Amount, AccountAddress Receiver) : AccountTransactionPayload
{
    /// <summary>
    /// The account transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TransactionType = (byte)Types.TransactionType.Transfer;

    /// <summary>
    /// The length of the payload serialized.
    /// </summary>
    private const uint BytesLength = sizeof(TransactionType) + AccountAddress.BytesLength + CcdAmount.BytesLength;

    /// <summary>
    /// Copies the "transfer" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver)
    {
        using var memoryStream = new MemoryStream((int)(BytesLength));
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(receiver.ToBytes());
        memoryStream.Write(amount.ToBytes());
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Create a "transfer" payload from a serialized as bytes.
    /// </summary>
    /// <param name="bytes">The "transfer" payload as bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(byte[] bytes, out (Transfer? , String? Error) output) {
        if (bytes.Length != BytesLength) {
            var msg = $"Invalid length in `Transfer.TryDeserial`. Expected {BytesLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        if (bytes[0] != TransactionType) {
			var msg = $"Invalid transaction type in `Transfer.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        };

        var accountBytes = bytes.Skip(1).Take((int) AccountAddress.BytesLength).ToArray();
        var accDeserial = AccountAddress.TryDeserial(accountBytes, out var account);

        if (!accDeserial) {
            output = (null, account.Item2);
            return false;
        };

        var amountBytes = bytes.Skip((int) AccountAddress.BytesLength + 1).ToArray();
        var amountDeserial = CcdAmount.TryDeserial(amountBytes, out var amount);

        if (!amountDeserial) {
            output = (null, amount.Item2);
            return false;
        };

        output = (new Transfer(amount.Item1.Value, account.Item1), null);
        return false;
    }

    public override ulong GetTransactionSpecificCost() => 300;

    public override byte[] ToBytes() => Serialize(this.Amount, this.Receiver);
}
