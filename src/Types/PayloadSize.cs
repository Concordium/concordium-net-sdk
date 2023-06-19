using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a payload size.
/// </summary>
/// <param name="Size">Payload size</param>
public readonly record struct PayloadSize(uint Size)
{
    public const uint BytesLength = sizeof(uint);

    /// <summary>
    /// Copies the payload size in big-endian format to a byte array.
    /// </summary>
    public byte[] ToBytes() => Serialization.ToBytes(this.Size);
}
