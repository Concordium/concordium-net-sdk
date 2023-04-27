using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

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
    public readonly Data Data;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterData"/> class.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    private RegisterData(Data data)
    {
        Data = data;
    }

    public override ulong GetBaseEnergyCost() => 300;

    /// <summary>
    /// Get the "register data" account transaction serialized to the binary format expected by the node.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    private static byte[] Serialize(Data data)
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
