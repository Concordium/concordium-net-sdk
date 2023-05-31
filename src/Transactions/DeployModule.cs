using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a "deploy module" account transaction.
///
/// Used for deploying smart contract modules on the chain.
/// </summary>
public record DeployModule : AccountTransactionPayload
{
    /// <summary>
    /// The deploy module transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TransactionType = (byte)AccountTransactionType.DeployModule;

    /// <summary>
    /// The versioned smart contract Wasm module to deploy.
    /// </summary>
    public WasmModule Source { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeployModule"/> class.
    /// </summary>
    /// <param name="source">The versioned module source of the module to deploy.</param>
    public DeployModule(WasmModule source) => this.Source = source;

    /// <summary>
    /// Copies the "deploy module" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="module">The smart contract Wasm module to be deployed.</param>
    private static byte[] Serialize(WasmModule module)
    {
        using var memoryStream = new MemoryStream((int)(
            sizeof(AccountTransactionType) +
            sizeof(uint) +
            module.Bytes.Length));
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(receiver.ToBytes());
        return memoryStream.ToArray();
    }

    public override ulong GetTransactionSpecificCost() => 300;

    public override byte[] ToBytes() => new byte[] { };
}
