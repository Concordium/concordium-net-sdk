using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Models a payload size.
/// </summary>
public struct PayloadSize
{
    public const UInt32 BytesLength = sizeof(UInt32);
    private UInt32 _value;

    public PayloadSize(UInt32 value)
    {
        _value = value;
    }

    public static implicit operator PayloadSize(UInt32 value)
    {
        return new PayloadSize(value);
    }

    public static implicit operator UInt32(PayloadSize byteIndex)
    {
        return byteIndex._value;
    }

    /// <summary>
    /// Get the payload size in the binary format expected by the node.
    /// </summary>
    public byte[] GetBytes()
    {
        return Serialization.GetBytes(_value);
    }
}
