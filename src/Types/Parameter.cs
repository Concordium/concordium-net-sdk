using System.Buffers.Binary;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Parameter to the init function or entrypoint.
/// </summary>
public sealed record Parameter(byte[] Param)
{
    /// <summary>
    /// Construct an empty smart contract parameter.
    /// </summary>
    public static Parameter Empty() => new(Array.Empty<byte>());

    private const uint MaxByteLength = 65535;
    /// <summary>
    /// Gets the serialized length (number of bytes) of the parameter.
    /// </summary>
    internal uint SerializedLength() => sizeof(ushort) + (uint)this.Param.Length;

    /// <summary>
    /// Gets the minimum serialized length (number of bytes) of the parameter.
    /// </summary>
    internal const uint MinSerializedLength = sizeof(ushort);

    internal static Parameter From(Grpc.V2.Parameter parameter) => new(parameter.Value.ToArray());

    /// <summary>
    /// Copies the parameters to a byte array which has the length preprended.
    /// </summary>
	public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.SerializedLength());
        memoryStream.Write(Serialization.ToBytes((ushort)this.Param.Length));
        memoryStream.Write(this.Param);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Create a parameter from a byte array.
    /// </summary>
    /// <param name="bytes">The serialized parameters.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (Parameter? Parameter, string? Error) output)
    {
        if (bytes.Length < MinSerializedLength)
        {
            var msg = $"Invalid length of input in `Parameter.TryDeserial`. Expected at least {MinSerializedLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        var sizeRead = BinaryPrimitives.ReadUInt16BigEndian(bytes);
        if (sizeRead > MaxByteLength)
        {
            var msg = $"Invalid length of input in `Parameter.TryDeserial`. The parameter size can be at most {MaxByteLength} bytes, found {bytes.Length}";
            output = (null, msg);
            return false;
        }

        var size = sizeof(ushort) + sizeRead;
        if (size > bytes.Length)
        {
            var msg = $"Invalid length of input in `Parameter.TryDeserial`. Expected array of size at least {size}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        let parameter = new Parameter(bytes.Slice(sizeof(ushort), sizeRead).ToArray());
        output = (parameter, null);
        return true;
    }

    /// <summary>
    /// Convert parameters to hex string.
    /// </summary>
    public string ToHexString() => Convert.ToHexString(this.Param).ToLowerInvariant();
}
