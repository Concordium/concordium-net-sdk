using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Models a "register data" transaction.
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
    private Data _data;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterData"/> class.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    private RegisterData(Data data)
    {
        _data = data;
    }

    /// <summary>
    /// Creates a new instance of the register data payload.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    public static AccountTransactionPayload<RegisterData> Create(Data data)
    {
        return new RegisterData(data);
    }

    public override ulong GetBaseEnergyCost() => 300;

    public override byte[] GetBytes()
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.WriteByte(TRANSACTION_TYPE);
        memoryStream.Write(_data.GetBytes());
        return memoryStream.ToArray();
    }
}
