using Concordium.Sdk.Types;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a "register data" transaction.
///
/// Used for registering data on-chain.
/// </summary>
public record RegisterData : AccountTransactionPayload<RegisterData>
{
    /// <summary>
    /// The account transaction type to be used in the serialized payload.
    /// </summary>
    private const byte TRANSACTION_TYPE = (byte)AccountTransactionType.RegisterData;

    /// <summary>
    /// The data to be registered on-chain.
    /// </summary>
    public readonly OnChainData Data;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterData"/> class.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    public RegisterData(OnChainData data)
    {
        Data = data;
    }

    public override ulong GetTransactionSpecificCost() => 300;

    /// <summary>
    /// Get the "register data" account transaction serialized to the binary format expected by the node.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    private static byte[] Serialize(OnChainData data)
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.WriteByte(TRANSACTION_TYPE);
        memoryStream.Write(data.GetBytes());
        return memoryStream.ToArray();
    }

    public override byte[] GetBytes()
    {
        return Serialize(Data);
    }
}
