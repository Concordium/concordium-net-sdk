using System.Buffers.Binary;

namespace Concordium.Sdk.Helpers;

/// <summary>
/// Helpers for serializing data.
/// </summary>
public class Serialization
{
    /// <summary>
    /// Gets bytes representing the specified <see cref="ulong"/> in big-endian format.
    /// </summary>
    public static byte[] GetBytes(ulong value)
    {
        var bytes = new byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        return bytes;
    }

    /// <summary>
    /// Gets bytes representing the specified <see cref="uint"/> in big-endian format.
    /// </summary>
    public static byte[] GetBytes(uint value)
    {
        var bytes = new byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        return bytes;
    }

    /// <summary>
    /// Gets bytes representing the specified <see cref="ushort"/> in big-endian format.
    /// </summary>
    public static byte[] GetBytes(ushort value)
    {
        var bytes = new byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        return bytes;
    }
}
