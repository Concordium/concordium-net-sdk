using Concordium.Sdk.Helpers;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>A deployment of a Wasm smart contract module.</summary>
/// <param name="Module">The smart contract module to be deployed.</param>
public sealed record DeployModule(VersionedModuleSource Module) : AccountTransactionPayload
{
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
            module.BytesLength
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
    public static bool TryDeserial(byte[] bytes, out (DeployModule? Module, String? Error) output) {
        if (bytes.Length <= 9) {
			var msg = $"Invalid input length in `DeployModule.TryDeserial`. expected at least 9, found {bytes.Length}";
            output = (null, msg);
            return false;
        }

        var deserialSuccess = VersionedModuleSourceFactory.TryDeserial(bytes.Skip(1).ToArray(), out var module);

        if (!deserialSuccess) {
            output = (null, module.Item2);
            return false;
        };
        if (bytes[0] != TransactionType) {
			var msg = $"Invalid transaction type in `DeployModule.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        }

        output = (new DeployModule(module.Item1), null);
        return true;
    }

    public override ulong GetTransactionSpecificCost() => 300;

    public override byte[] ToBytes() => Serialize(this.Module);
}

