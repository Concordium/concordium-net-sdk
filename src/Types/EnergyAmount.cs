using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an amount of energy.
/// </summary>
/// <param name="Value">Value of the energy amount.</param>
public readonly record struct EnergyAmount(ulong Value)
{
    ///<summary>Byte length of Energy. Used for serialization.</summary>
    public const uint BytesLength = sizeof(ulong);

    /// <summary>
    /// Copies the energy amount represented in big-endian format to byte array.
    /// </summary>
    public byte[] ToBytes() => Serialization.ToBytes(this.Value);

    internal static EnergyAmount From(Grpc.V2.Energy energy) => new(energy.Value);

    /// <summary>
    /// Add Energy amounts.
    /// </summary>
    /// <exception cref="ArgumentException">The result odoes not fit in <see cref="ulong"/></exception>
    public static EnergyAmount operator +(EnergyAmount a, EnergyAmount b)
    {
        try
        {
            return new EnergyAmount(checked(a.Value + b.Value));
        }
        catch (OverflowException e)
        {
            throw new ArgumentException(
                $"The result of {a.Value} + {b.Value} does not fit in UInt64.", e
            );
        }
    }

    /// <summary>
    /// Subtract Energy amounts.
    /// </summary>
    /// <exception cref="ArgumentException">The result does not fit in <see cref="ulong"/></exception>
    public static EnergyAmount operator -(EnergyAmount a, EnergyAmount b)
    {
        try
        {
            return new EnergyAmount(checked(a.Value - b.Value));
        }
        catch (OverflowException e)
        {
            throw new ArgumentException(
                $"The result of {a.Value} - {b.Value} does not fit in UInt64.", e
            );
        }
    }

}
