using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a "register data" account transaction.
///
/// Used for registering data on-chain.
/// </summary>
/// <param name="Data">The data to be registered on-chain.</param>
public sealed record RegisterData(OnChainData Data) : AccountTransactionPayload
{
    /// <summary>
    /// The account transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TransactionType = (byte)AccountTransactionType.RegisterData;

    public override ulong GetTransactionSpecificCost() => 300;

    /// <summary>
    /// Copies the "register data" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    private static byte[] Serialize(OnChainData data)
    {
        var buffer = data.ToBytes();
        var size = sizeof(AccountTransactionType) + buffer.Length;
        using var memoryStream = new MemoryStream(size);
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(buffer);
        return memoryStream.ToArray();
    }

    public override byte[] ToBytes() => Serialize(this.Data);
}
