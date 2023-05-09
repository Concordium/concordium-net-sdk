using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an amount of energy.
/// </summary>
public readonly struct EnergyAmount
{
    public const uint BytesLength = sizeof(ulong);
    public readonly ulong Value { get; init; }

    public EnergyAmount(ulong value) => this.Value = value;

    public static implicit operator EnergyAmount(ulong value) => new(value);

    public static implicit operator ulong(EnergyAmount value) => value.Value;

    /// <summary>
    /// Get the energy amount in the binary format expected by the node.
    /// </summary>
    public byte[] GetBytes() => Serialization.GetBytes(this.Value);
}
