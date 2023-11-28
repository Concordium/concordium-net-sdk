using System.Buffers.Binary;

namespace Concordium.Sdk.Helpers;

/// <summary>
/// Helpers for deserializing data.
/// </summary>
public static class Deserial
{
    /// <summary>
    /// Creates a ushort from a byte array.
    /// </summary>
    public static bool TryDeserialU16(byte[] input, int offset, out (ushort? Uint, string? Error) output)
    {
        if (input.Length < sizeof(ushort))
        {
            var msg = $"Invalid length in TryDeserialU32. Must be longer than {sizeof(ushort)}, but was {input.Length}";
            output = (null, msg);
            return false;
        }

        var offset_input = input.Skip(offset).ToArray();

        var bytes = offset_input.Take(sizeof(ushort)).ToArray();

        output = (BinaryPrimitives.ReadUInt16BigEndian(bytes), null);
        return true;
    }

    /// <summary>
    /// Creates a uint from a byte array.
    /// </summary>
    public static bool TryDeserialU32(byte[] input, int offset, out (uint? Uint, string? Error) output)
    {
        if (input.Length < sizeof(uint))
        {
            var msg = $"Invalid length in TryDeserialU32. Must be longer than 4, but was {input.Length}";
            output = (null, msg);
            return false;
        }

        var offset_input = input.Skip(offset).ToArray();

        var bytes = offset_input.Take(sizeof(uint)).ToArray();

        output = (BinaryPrimitives.ReadUInt32BigEndian(bytes), null);
        return true;
    }

    /// <summary>
    /// Creates a ulong from a byte array.
    /// </summary>
    public static bool TryDeserialU64(byte[] input, int offset, out (ulong? Ulong, string? Error) output)
    {
        if (input.Length < sizeof(ulong))
        {
            var msg = $"Invalid length in TryDeserialU32. Must be longer than {sizeof(ulong)}, but was {input.Length}";
            output = (null, msg);
            return false;
        }

        var offset_input = input.Skip(offset).ToArray();

        var bytes = offset_input.Take(sizeof(ulong)).ToArray();

        output = (BinaryPrimitives.ReadUInt64BigEndian(bytes), null);
        return true;
    }
}


