using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an amount of energy.
/// </summary>
/// <param name="Value">Value of the energy amount.</param>
public readonly record struct EnergyAmount(ulong Value)
{
    public const uint BytesLength = sizeof(ulong);

    /// <summary>
    /// Copies the energy amount represented in big-endian format to byte array.
    /// </summary>
    public byte[] ToBytes() => Serialization.ToBytes(this.Value);
}
