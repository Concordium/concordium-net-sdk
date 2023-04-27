using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Models an energy amount.
/// </summary>
public readonly struct EnergyAmount
{
    public const UInt32 BytesLength = sizeof(UInt64);
    private readonly UInt64 Value { get; init; }

    public EnergyAmount(UInt64 value)
    {
        Value = value;
    }

    public static implicit operator EnergyAmount(UInt64 value)
    {
        return new EnergyAmount(value);
    }

    public static implicit operator UInt64(EnergyAmount byteIndex)
    {
        return byteIndex.Value;
    }

    /// <summary>
    /// Get the energy amount in the binary format expected by the node.
    /// </summary>
    public byte[] GetBytes()
    {
        return Serialization.GetBytes(Value);
    }
}
