namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a CCD amount.
///
/// Note that 1_000_000 µCCD is equal to 1 CCD.
/// </summary>
public readonly struct CcdAmount : IEquatable<CcdAmount>
{
    public const int BytesLength = 8;

    /// <summary>
    /// Conversion factor, 1_000_000 µCCD = 1 CCD.
    /// </summary>
    public const UInt64 MicroCcdPerCcd = 1_000_000;

    /// <summary>
    /// The amount in µCCD.
    /// </summary>
    public readonly UInt64 MicroCcd;

    /// <summary>
    /// Initializes a new instance of the <see cref="CcdAmount"/> class.
    /// </summary>
    /// <param name="microCcd">The amount in µCCD.</param>
    private CcdAmount(UInt64 microCcd)
    {
        MicroCcd = microCcd;
    }

    /// <summary>
    /// Get a formatted string representing the amount in µCCD.
    /// </summary>
    public string GetFormattedMicroCcd()
    {
        return $"{MicroCcd}";
    }

    /// <summary>
    /// Get a formatted string representing the amount in CCD.
    /// </summary>
    public string GetFormattedCcd()
    {
        return $"{MicroCcd / (decimal)MicroCcdPerCcd}";
    }

    /// <summary>
    /// Creates an instance from a µCCD amount represented as an integer.
    /// </summary>
    /// <param name="microCcd">µCCD amount represented as an integer.</param>
    public static CcdAmount FromMicroCcd(UInt64 microCcd)
    {
        return new CcdAmount(microCcd);
    }

    /// <summary>
    /// Creates an instance from a CCD amount represented by an integer.
    /// </summary>
    /// <param name="ccd">CCD amount represented by an integer.</param>
    /// <exception cref="ArgumentException">If the CCD amount in µCCD does not fit in <see cref="UInt64"/></exception>
    public static CcdAmount FromCcd(UInt64 ccd)
    {
        try
        {
            return new CcdAmount(checked(ccd * MicroCcdPerCcd));
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {ccd} CCD * {MicroCcdPerCcd} µCCD/CCD does not fit in UInt64."
            );
        }
    }

    /// <summary>
    /// Add CCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">If the result odoes not fit in <see cref="UInt64"/></exception>
    public static CcdAmount operator +(CcdAmount a, CcdAmount b)
    {
        try
        {
            UInt64 newAmount = checked(a.MicroCcd + b.MicroCcd);
            return CcdAmount.FromMicroCcd(newAmount);
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {a.MicroCcd} + {b.MicroCcd} does not fit in UInt64."
            );
        }
    }

    /// <summary>
    /// Subtract CCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">If the result does not fit in <see cref="UInt64"/></exception>
    public static CcdAmount operator -(CcdAmount a, CcdAmount b)
    {
        try
        {
            UInt64 newAmount = checked(a.MicroCcd - b.MicroCcd);
            return CcdAmount.FromMicroCcd(newAmount);
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {a.MicroCcd} + {b.MicroCcd} does not fit in UInt64."
            );
        }
    }

    public bool Equals(CcdAmount other)
    {
        return MicroCcd == other.MicroCcd;
    }

    public override bool Equals(Object? obj)
    {
        return obj is CcdAmount other && Equals(other);
    }

    public override int GetHashCode()
    {
        return MicroCcd.GetHashCode();
    }
}
