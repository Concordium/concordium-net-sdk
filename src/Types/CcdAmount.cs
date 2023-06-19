using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a CCD amount.
///
/// Note that 1_000_000 µCCD is equal to 1 CCD.
/// </summary>
public readonly record struct CcdAmount
{
    public const uint BytesLength = 8;

    /// <summary>
    /// Conversion factor, 1_000_000 µCCD = 1 CCD.
    /// </summary>
    public const ulong MicroCcdPerCcd = 1_000_000;

    /// <summary>
    /// The amount in µCCD.
    /// </summary>
    public ulong Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CcdAmount"/> class.
    /// </summary>
    /// <param name="microCcd">The amount in µCCD.</param>
    private CcdAmount(ulong microCcd) => this.Value = microCcd;

    /// <summary>
    /// Get a formatted string representing the amount in µCCD.
    /// </summary>
    public string GetFormattedMicroCcd() => $"{this.Value}";

    /// <summary>
    /// Get a formatted string representing the amount in CCD.
    /// </summary>
    public string GetFormattedCcd() => $"{this.Value / (decimal)MicroCcdPerCcd}";

    /// <summary>
    /// Creates an instance from a µCCD amount represented as an integer.
    /// </summary>
    /// <param name="microCcd">µCCD amount represented as an integer.</param>
    public static CcdAmount FromMicroCcd(ulong microCcd) => new(microCcd);

    /// <summary>
    /// Creates an instance from a CCD amount represented by an integer.
    /// </summary>
    /// <param name="ccd">CCD amount represented by an integer.</param>
    /// <exception cref="ArgumentException">The CCD amount in µCCD does not fit in <see cref="ulong"/></exception>
    public static CcdAmount FromCcd(ulong ccd)
    {
        try
        {
            return new CcdAmount(checked(ccd * MicroCcdPerCcd));
        }
        catch (OverflowException e)
        {
            throw new ArgumentException(
                $"The result of {ccd} CCD * {MicroCcdPerCcd} µCCD/CCD does not fit in UInt64.", e
            );
        }
    }

    /// <summary>
    /// Add CCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">The result odoes not fit in <see cref="ulong"/></exception>
    public static CcdAmount operator +(CcdAmount a, CcdAmount b)
    {
        try
        {
            var newAmount = checked(a.Value + b.Value);
            return FromMicroCcd(newAmount);
        }
        catch (OverflowException e)
        {
            throw new ArgumentException(
                $"The result of {a.Value} + {b.Value} does not fit in UInt64.", e
            );
        }
    }

    /// <summary>
    /// Subtract CCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">The result does not fit in <see cref="ulong"/></exception>
    public static CcdAmount operator -(CcdAmount a, CcdAmount b)
    {
        try
        {
            var newAmount = checked(a.Value - b.Value);
            return FromMicroCcd(newAmount);
        }
        catch (OverflowException e)
        {
            throw new ArgumentException(
                $"The result of {a.Value} - {b.Value} does not fit in UInt64.", e
            );
        }
    }

    /// <summary>
    /// Copies the CCD amuunt represented in big-endian format to  byte array.
    /// </summary>
    public byte[] ToBytes() => Serialization.ToBytes(this.Value);
}
