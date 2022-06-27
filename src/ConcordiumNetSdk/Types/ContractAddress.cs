using System.Buffers.Binary;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents the information about a contract address.
/// </summary>
public class ContractAddress
{
    /// <summary>
    /// Gets the serialized bytes length.
    /// </summary>
    public const int BytesLength = 16;

    private ContractAddress(ulong index, ulong subIndex)
    {
        Index = index;
        SubIndex = subIndex;
    }
    
    /// <summary>
    /// Gets or initiates the index.
    /// </summary>
    public ulong Index { get; }

    /// <summary>
    /// Gets or initiates the sub index.
    /// </summary>
    public ulong SubIndex { get; }

    /// <summary>
    /// Creates an instance of contract address.
    /// </summary>
    /// <param name="index">the index value.</param>
    /// <param name="subIndex">the sub index value.</param>
    public static ContractAddress Create(ulong index, ulong subIndex)
    {
        return new ContractAddress(index, subIndex);
    }
    
    /// <summary>
    /// Serializes contract address to byte format.
    /// </summary>
    /// <returns><see cref="T:byte[]"/> - serialized contract address in byte format.</returns>
    public byte[] SerializeToBytes(bool useLittleEndian = false)
    {
        byte[] bytes = new byte[BytesLength];
        Span<byte> buffer = bytes;
        if (useLittleEndian)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(0, 8), Convert.ToUInt64(Index));
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(8, 8), Convert.ToUInt64(SubIndex));
        }
        else
        {
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(0, 8), Convert.ToUInt64(Index));
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(8, 8), Convert.ToUInt64(SubIndex));
        }

        return bytes;
    }
}
