using Concordium.Sdk.Exceptions;
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
    /// Prepares the account transaction payload for signing.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="sequenceNumber">Account sequence number to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    public PreparedAccountTransaction Prepare(
        AccountAddress sender,
        AccountSequenceNumber sequenceNumber,
        Expiry expiry
    ) => new(sender, sequenceNumber, expiry, this._transactionCost, this);

    /// <summary>
    /// The transaction specific cost for submitting this type of
    /// transaction to the chain.
    ///
    /// This should reflect the transaction-specific costs defined here:
    /// https://github.com/Concordium/concordium-base/blob/78f557b8b8c94773a25e4f86a1a92bc323ea2e3d/haskell-src/Concordium/Cost.hs
    /// </summary>
    private readonly EnergyAmount _transactionCost = new(300);

    /// <summary>
    /// Copies the "transfer" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="amount">Amount to send.</param>
    /// <param name="receiver">Address of the receiver account to which the amount will be sent.</param>
    private static byte[] Serialize(CcdAmount amount, AccountAddress receiver)
    {
        using var memoryStream = new MemoryStream((int)BytesLength);
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
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (Transfer? Transfer, string? Error) output)
    {
        if (bytes.Length != BytesLength)
        {
            var msg = $"Invalid length in `Transfer.TryDeserial`. Expected {BytesLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        if (bytes[0] != TransactionType)
        {
            var msg = $"Invalid transaction type in `Transfer.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        };

        var accountBytes = bytes[sizeof(TransactionType)..];
        if (!AccountAddress.TryDeserial(accountBytes, out var account))
        {
            output = (null, account.Error);
            return false;
        };

        var amountBytes = bytes[((int)AccountAddress.BytesLength + sizeof(TransactionType))..];
        if (!CcdAmount.TryDeserial(amountBytes, out var amount))
        {
            output = (null, amount.Error);
            return false;
        };

        if (amount.Amount == null || account.AccountAddress == null)
        {
            throw new DeserialInvalidResultException();
        }

        output = (new Transfer(amount.Amount.Value, account.AccountAddress), null);
        return true;
    }

    public override byte[] ToBytes() => Serialize(this.Amount, this.Receiver);
}
