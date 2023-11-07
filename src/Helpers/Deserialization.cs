using System.Buffers.Binary;

namespace Concordium.Sdk.Helpers;

/// <summary>
/// Error on deserialization.
/// </summary>
public enum DeserialErr
{
    TooShort,
	InvalidModuleVersion,
}

/// <summary>
/// Helpers for deserializing data.
/// </summary>
public static class Deserial
{
    /// <summary>
    /// Creates a uint from a byte array.
    /// </summary>
    public static bool TryDeserialU32(byte[] input, int offset, out (uint? Uint, DeserialErr? Error) output)
    {
		var offset_input = input.Skip(offset).ToArray();

        if (offset_input.Length < 4) {
			output = (null, DeserialErr.TooShort);
			return false;
		}

		var bytes = offset_input.Take(4).ToArray();

        output = (BinaryPrimitives.ReadUInt32BigEndian(bytes), null);
		return true;
    }

	private static void PrintBytes(String msg, byte[] bytes) {
		Console.WriteLine(msg);
		foreach (byte b in bytes) {
			Console.Write(b);
			Console.Write(" ");
		}
		Console.Write("\n");
	} 
}


