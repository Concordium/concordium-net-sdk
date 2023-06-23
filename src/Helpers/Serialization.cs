using System.Buffers.Binary;

namespace Concordium.Sdk.Helpers;

/// <summary>
/// Helpers for serializing data.
/// </summary>
public static class Serialization
{
    /// <summary>
    /// Copies the bytes representing the specified <see cref="ulong"/> in big-endian format to a byte array.
    /// </summary>
    public static byte[] ToBytes(ulong value)
    {
        var bytes = new byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        return bytes;
    }

    /// <summary>
    /// Copies the bytes representing the specified <see cref="ulong"/> in big-endian format to a byte array.
    /// </summary>
    public static byte[] ToBytes(uint value)
    {
        var bytes = new byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        return bytes;
    }

    /// <summary>
    /// Copies the bytes representing the specified <see cref="ulong"/> in big-endian format to a byte array.
    /// </summary>
    public static byte[] ToBytes(ushort value)
    {
        var bytes = new byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        return bytes;
    }
}
