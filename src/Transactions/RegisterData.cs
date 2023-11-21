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
    private const byte TransactionType = (byte)Types.TransactionType.RegisterData;

    public override ulong GetTransactionSpecificCost() => 300;

    /// <summary>
    /// Copies the "register data" account transaction in the binary format expected by the node to a byte array.
    /// </summary>
    /// <param name="data">The data to be registered on-chain.</param>
    private static byte[] Serialize(OnChainData data)
    {
        var buffer = data.ToBytes();
        var size = sizeof(TransactionType) + buffer.Length;
        using var memoryStream = new MemoryStream(size);
        memoryStream.WriteByte(TransactionType);
        memoryStream.Write(buffer);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Create a "register data" payload from a serialized as bytes.
    /// </summary>
    /// <param name="bytes">The payload as bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(byte[] bytes, out (RegisterData?, string? Error) output)
    {
        var minSize = sizeof(TransactionType);
        if (bytes.Length < minSize)
        {
            var msg = $"Invalid length in `TransferWithMemo.TryDeserial`. Expected at least {minSize}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        if (bytes[0] != TransactionType)
        {
            var msg = $"Invalid transaction type in `Transfer.TryDeserial`. expected {TransactionType}, found {bytes[0]}";
            output = (null, msg);
            return false;
        };

        var memoBytes = bytes.Skip(sizeof(TransactionType)).ToArray();
        var memoDeserial = OnChainData.TryDeserial(memoBytes, out var memo);

        if (!memoDeserial)
        {
            output = (null, memo.Error);
            return false;
        };

        output = (new RegisterData(memo.accountAddress), null);
        return true;
    }

    public override byte[] ToBytes() => Serialize(this.Data);
}
