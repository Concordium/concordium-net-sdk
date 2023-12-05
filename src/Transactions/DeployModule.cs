using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>A deployment of a Wasm smart contract module.</summary>
/// <param name="Module">The smart contract module to be deployed.</param>
public sealed record DeployModule(VersionedModuleSource Module) : AccountTransactionPayload
{
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

    private readonly EnergyAmount _transactionCost = new(Module.GetBytesLength() / 10);

    /// <summary>The account transaction type to be used in the serialized payload.</summary>
    private const byte TransactionType = (byte)Types.TransactionType.DeployModule;

    /// <summary>
    /// Copies the "deploy module" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="module">The smart contract module to be deployed.</param>
    private static byte[] Serialize(VersionedModuleSource module)
    {
        using var memoryStream = new MemoryStream((int)(
            sizeof(TransactionType) +
            module.GetBytesLength()
        ));
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(module.ToBytes());
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Create a "deploy module" payload from a serialized as bytes.
    /// </summary>
    /// <param name="bytes">The "deploy module" payload as bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (DeployModule? Module, string? Error) output)
    {
        var minLength = sizeof(TransactionType) + (2 * sizeof(int));
        if (bytes.Length < minLength)
        {
            var msg = $"Invalid input length in `DeployModule.TryDeserial`. expected at least {minLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        }

        if (bytes[0] != TransactionType)
        {
            var msg = $"Invalid transaction type in `DeployModule.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        }

        var deserialSuccess = VersionedModuleSourceFactory.TryDeserial(bytes[sizeof(TransactionType)..], out var module);
        if (!deserialSuccess)
        {
            output = (null, module.Error);
            return false;
        };

        if (module.VersionedModuleSource == null)
        {
            throw new DeserialInvalidResultException();
        }

        output = (new DeployModule(module.VersionedModuleSource), null);
        return true;
    }

    public override byte[] ToBytes() => Serialize(this.Module);
}

