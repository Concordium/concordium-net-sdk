using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>A deployment of a Wasm smart contract module.</summary>
/// <param name="Amount">Amount of CCD to send to the instance.</param>
/// <param name="Address">Address of the instance to update.</param>
/// <param name="ReceiveName">Name of the receive function to call to update the instance.</param>
/// <param name="Parameter">Name of the receive function to call to update the instance.</param>
public sealed record UpdateContract(CcdAmount Amount, ContractAddress Address, ReceiveName ReceiveName, Parameter Parameter) : AccountTransactionPayload
{
    /// <summary>
    /// Prepares the account transaction payload for signing.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="sequenceNumber">Account sequence number to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    /// <param name="energy">
    /// The amount of energy that can be used for contract execution.
    /// The base energy amount for transaction verification will be added to this cost.
    /// </param>
    public PreparedAccountTransaction Prepare(
        AccountAddress sender,
        AccountSequenceNumber sequenceNumber,
        Expiry expiry,
        EnergyAmount energy
    ) => new(sender, sequenceNumber, expiry, new(_baseCost.Value + energy.Value), this);

    /// <summary>
    /// The transaction specific cost for submitting this type of
    /// transaction to the chain.
    ///
    /// This should reflect the transaction-specific costs defined here:
    /// https://github.com/Concordium/concordium-base/blob/78f557b8b8c94773a25e4f86a1a92bc323ea2e3d/haskell-src/Concordium/Cost.hs
    /// </summary>
    private static readonly EnergyAmount _baseCost = new(300);

    /// <summary>The account transaction type to be used in the serialized payload.</summary>
    private const byte TransactionType = (byte)Types.TransactionType.Update;

    /// <summary>
    /// Gets the size (number of bytes) of the payload.
    /// </summary>
    internal override PayloadSize Size() => new(sizeof(TransactionType) + CcdAmount.BytesLength + ContractAddress.BytesLength + this.ReceiveName.SerializedLength() + this.Parameter.SerializedLength());

    /// <summary>
    /// Create a "update contract" payload from a serialized as bytes.
    /// </summary>
    /// <param name="bytes">The "update contract" payload as bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (UpdateContract? payload, string? Error) output)
    {
        var minLength = sizeof(TransactionType) + CcdAmount.BytesLength + ContractAddress.BytesLength + ReceiveName.MinSerializedLength + Parameter.MinSerializedLength;
        if (bytes.Length < minLength)
        {
            var msg = $"Invalid input length in `UpdateContract.TryDeserial`. Expected at least {minLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        }

        if (bytes[0] != TransactionType)
        {
            var msg = $"Invalid transaction type in `UpdateContract.TryDeserial`. Expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        }

        var remaining_bytes = bytes[sizeof(TransactionType)..];

        if (!CcdAmount.TryDeserial(remaining_bytes, out var amount))
        {
            output = (null, amount.Error);
            return false;
        };
        remaining_bytes = remaining_bytes[(int)CcdAmount.BytesLength..];

        if (!ContractAddress.TryDeserial(remaining_bytes, out var address))
        {
            output = (null, address.Error);
            return false;
        };
        remaining_bytes = remaining_bytes[(int)ContractAddress.BytesLength..];

        if (!ReceiveName.TryDeserial(remaining_bytes, out var receiveName) || receiveName.receiveName == null)
        {
            output = (null, receiveName.Error);
            return false;
        };
        remaining_bytes = remaining_bytes[(int)receiveName.receiveName.SerializedLength()..];

        if (!Parameter.TryDeserial(remaining_bytes, out var parameter))
        {
            output = (null, parameter.Error);
            return false;
        };

        if (amount.Amount == null || address.Address == null || receiveName.receiveName == null || parameter.Parameter == null)
        {
            var msg = $"Unexpected null pointer when deserializing.";
            output = (null, msg);
            return false;
        }

        var payload = new UpdateContract(amount.Amount.Value, address.Address, receiveName.receiveName, parameter.Parameter);
        output = (payload, null);
        return true;

    }

    /// <summary>
    /// Copies the "update contract" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    public override byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.Size().Size); // Safe to cast since a payload will never be large enough for this to overflow.
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(this.Amount.ToBytes());
        memoryStream.Write(this.Address.ToBytes());
        memoryStream.Write(this.ReceiveName.ToBytes());
        memoryStream.Write(this.Parameter.ToBytes());
        return memoryStream.ToArray();
    }
}

