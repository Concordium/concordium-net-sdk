using System.Buffers.Binary;

namespace Concordium.Sdk.Helpers;

/// <summary>
/// Error on deserialization.
/// </summary>
public enum DeserialErr
{
    TooShort,
	InvalidModuleVersion,
	InvalidTransactionType,
	InternalError,
	InvalidLength,
}



/// <summary>
/// Helpers for deserializing data.
/// </summary>
public static class Deserial
{
    /// <summary>
    /// Creates a uint from a byte array.
    /// </summary>
    public static bool TryDeserialU32(byte[] input, int offset, out (uint? Uint, String? Error) output)
    {
        if (input.Length < 4) {
			var msg = $"Invalid length in TryDeserialU32. Must be longer than 4, but was {input.Length}";
			output = (null, msg);
			return false;
		}

		var offset_input = input.Skip(offset).ToArray();

		var bytes = offset_input.Take(4).ToArray();

        output = (BinaryPrimitives.ReadUInt32BigEndian(bytes), null);
		return true;
    }

    /// <summary>
    /// Creates a uint from a byte array.
    /// </summary>
    public static bool TryDeserialU64(byte[] input, int offset, out (ulong? Ulong, String? Error) output)
    {
        if (input.Length < 8) {
			var msg = $"Invalid length in TryDeserialU32. Must be longer than 4, but was {input.Length}";
			output = (null, msg);
			return false;
		}

		var offset_input = input.Skip(offset).ToArray();

		var bytes = offset_input.Take(8).ToArray();

        output = (BinaryPrimitives.ReadUInt64BigEndian(bytes), null);
		return true;
    }

	// TODO: Debug tool remove
	private static void PrintBytes(String msg, byte[] bytes) {
		Console.WriteLine(msg);
		foreach (byte b in bytes) {
			Console.Write(b);
			Console.Write(" ");
		}
		Console.Write("\n");
	} 
}


