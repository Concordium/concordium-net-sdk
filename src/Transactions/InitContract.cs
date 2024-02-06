using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents an "init_contract" transaction.
///
/// Used for initializing deployed smart contracts.
/// </summary>
/// <param name="Amount">Deposit this amount of CCD.</param>
/// <param name="ModuleRef">The smart contract module reference.</param>
/// <param name="ContractName">The init name of the smart contract.</param>
/// <param name="Parameter">The parameters for the smart contract.</param>
public sealed record InitContract(CcdAmount Amount, ModuleReference ModuleRef, ContractName ContractName, Parameter Parameter) : AccountTransactionPayload
{
    /// <summary>
    /// The init contract transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TransactionType = (byte)Types.TransactionType.InitContract;

    /// <summary>
    /// The minimum serialized length in the serialized payload.
    /// </summary>
	internal const uint MinSerializedLength =
        CcdAmount.BytesLength +
        Hash.BytesLength +
        ContractName.MinSerializedLength +
        Parameter.MinSerializedLength;

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
    ) => new(sender, sequenceNumber, expiry, energy, this);

    /// <summary>
    /// The base transaction specific cost for submitting this type of
    /// transaction to the chain.
    ///
    /// This should reflect the transaction-specific costs defined here:
    /// https://github.com/Concordium/concordium-base/blob/78f557b8b8c94773a25e4f86a1a92bc323ea2e3d/haskell-src/Concordium/Cost.hs
    /// </summary>
    private const ushort TrxBaseCost = 300;

    /// <summary>
    /// Gets the size (number of bytes) of the payload.
    /// </summary>
    internal override PayloadSize Size() => new(
        sizeof(TransactionType) +
        CcdAmount.BytesLength +
        Hash.BytesLength +
        this.ContractName.SerializedLength() +
        this.Parameter.SerializedLength());

    /// <summary>
    /// Deserialize a "InitContract" payload from a serialized byte array.
    /// </summary>
    /// <param name="bytes">The serialized InitContract payload.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (InitContract? InitContract, string? Error) output)
    {
        if (bytes.Length < MinSerializedLength)
        {
            var msg = $"Invalid length in `InitContract.TryDeserial`. Expected at least {MinSerializedLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        if (bytes[0] != TransactionType)
        {
            var msg = $"Invalid transaction type in `InitContract.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        };

        var amountBytes = bytes[sizeof(TransactionType)..];
        if (!CcdAmount.TryDeserial(amountBytes, out var amount))
        {
            output = (null, amount.Error);
            return false;
        };

        var refBytes = bytes[(int)(CcdAmount.BytesLength + sizeof(TransactionType))..];
        if (!ModuleReference.TryDeserial(refBytes, out var moduleRef))
        {
            output = (null, moduleRef.Error);
            return false;
        };

        var nameBytes = bytes[(int)(Hash.BytesLength + CcdAmount.BytesLength + sizeof(TransactionType))..];
        if (!ContractName.TryDeserial(nameBytes, out var name))
        {
            output = (null, name.Error);
            return false;
        };
        if (name.ContractName == null)
        {
            var msg = $"Name was null, but did not produce an error";
            output = (null, msg);
            return false;
        }

        var paramBytes = bytes[(int)(name.ContractName.SerializedLength() + Hash.BytesLength + CcdAmount.BytesLength + sizeof(TransactionType))..];
        if (!Parameter.TryDeserial(paramBytes, out var param))
        {
            output = (null, param.Error);
            return false;
        };

        if (amount.Amount == null || moduleRef.Ref == null || param.Parameter == null)
        {
            var msg = $"Amount, ModuleRef or Parameter were null, but did not produce an error";
            output = (null, msg);
            return false;
        }

        output = (new InitContract(amount.Amount.Value, moduleRef.Ref, name.ContractName, param.Parameter), null);
        return true;
    }

    /// <summary>
    /// Copies the "init_contract" transaction in the binary format expected by the node to a byte array.
    /// </summary>
    public override byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.Size().Size);
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(this.Amount.ToBytes());
        memoryStream.Write(this.ModuleRef.ToBytes());
        memoryStream.Write(this.ContractName.ToBytes());
        memoryStream.Write(this.Parameter.ToBytes());
        return memoryStream.ToArray();
    }
}
