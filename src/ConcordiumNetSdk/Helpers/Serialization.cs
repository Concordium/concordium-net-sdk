using System.Buffers.Binary;

namespace ConcordiumNetSdk.Helpers;

/// <summary>
/// Helpers for serializing data.
/// </summary>
public class Serialization
{
    public static byte[] GetBytes(UInt64 value)
    {
        var bytes = new byte[sizeof(UInt64)];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        return bytes;
    }

    public static byte[] GetBytes(UInt32 value)
    {
        var bytes = new byte[sizeof(UInt32)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        return bytes;
    }

    public static byte[] GetBytes(UInt16 value)
    {
        var bytes = new byte[sizeof(UInt16)];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        return bytes;
    }
}
