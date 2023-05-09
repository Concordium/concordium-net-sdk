using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents a payload size.
/// </summary>
public struct PayloadSize
{
    public const uint BytesLength = sizeof(uint);
    private readonly uint _value;

    public PayloadSize(uint value) => this._value = value;

    public static implicit operator PayloadSize(uint value) => new(value);

    public static implicit operator uint(PayloadSize byteIndex) => byteIndex._value;

    /// <summary>
    /// Get the payload size in the binary format expected by the node.
    /// </summary>
    public byte[] GetBytes() => Serialization.GetBytes(this._value);
}
