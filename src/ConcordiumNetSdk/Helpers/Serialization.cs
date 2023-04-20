using System.Buffers.Binary;

namespace ConcordiumNetSdk.Helpers;

/// <summary>
/// Helpers for serializing data.
/// </summary>
public class Serialization
{
    public static byte[] GetBytes(UInt64 value, bool useLittleEndian = false)
    {
        var bytes = new byte[sizeof(UInt64)];

        if (useLittleEndian)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        }

        return bytes;
    }

    public static byte[] GetBytes(UInt32 value, bool useLittleEndian = false)
    {
        var bytes = new byte[sizeof(UInt32)];

        if (useLittleEndian)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        }

        return bytes;
    }

    public static byte[] GetBytes(UInt16 value, bool useLittleEndian = false)
    {
        var bytes = new byte[sizeof(UInt16)];

        if (useLittleEndian)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        }

        return bytes;
    }
}
