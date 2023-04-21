namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents an amount in µCCD.
/// Note that 1_000_000 µCCD is equal to 1 CCD.
/// </summary>
public readonly struct MicroCCDAmount : IEquatable<MicroCCDAmount>
{
    public const int BytesLength = 8;

    /// <summary>
    /// Conversion factor, 1_000_000 µCCD = 1 CCD.
    /// </summary>
    private const UInt64 ONE_MILLION = 1_000_000;

    /// <summary>
    /// The amount in µCCD.
    /// </summary>
    private readonly UInt64 _microCcd;

    /// <summary>
    /// Initializes a new instance of the <see cref="MicroCCDAmount"/> class.
    /// </summary>
    /// <param name="microCcd">The amount in µCCD.</param>
    private MicroCCDAmount(UInt64 microCcd)
    {
        _microCcd = microCcd;
    }

    /// <summary>
    /// Get µCCD amount.
    /// </summary>
    public UInt64 GetMicroCcdValue()
    {
        return _microCcd;
    }

    /// <summary>
    /// Get a formatted string representing the amount in µCCD.
    /// </summary>
    public string GetFormattedMicroCcd()
    {
        return $"{_microCcd}";
    }

    /// <summary>
    /// Get a formatted string representing the amount in CCD.
    /// </summary>
    public string GetFormattedCcd()
    {
        return $"{_microCcd / (decimal)ONE_MILLION}";
    }

    /// <summary>
    /// Creates an instance from a µCCD amount represented by an integer.
    /// </summary>
    /// <param name="microCcd">µCCD amount represented by an integer.</param>
    public static MicroCCDAmount FromMicroCcd(UInt64 microCcd)
    {
        return new MicroCCDAmount(microCcd);
    }

    /// <summary>
    /// Creates an instance from a CCD amount represented by an integer.
    /// </summary>
    /// <param name="ccd">CCD amount represented by an integer.</param>
    /// <exception cref="ArgumentException">If the result does not fit in <see cref="UInt64"/></exception>
    public static MicroCCDAmount FromCcd(UInt64 ccd)
    {
        try
        {
            return new MicroCCDAmount(checked(ccd * ONE_MILLION));
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {ccd} CCD * {ONE_MILLION} does not fit in UInt64."
            );
        }
    }

    /// <summary>
    /// Creates an instance with the zero amount.
    /// </summary>
    public static MicroCCDAmount Zero()
    {
        return new MicroCCDAmount(0);
    }

    /// <summary>
    /// Add µCCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">If the result odoes not fit in <see cref="UInt64"/></exception>
    public static MicroCCDAmount operator +(MicroCCDAmount a, MicroCCDAmount b)
    {
        try
        {
            UInt64 newAmount = checked(a.GetMicroCcdValue() + b.GetMicroCcdValue());
            return MicroCCDAmount.FromCcd(newAmount);
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {a.GetMicroCcdValue()} + {b.GetMicroCcdValue()} does not fit in UInt64."
            );
        }
    }

    /// <summary>
    /// Subtract µCCD amounts.
    /// </summary>
    /// <exception cref="ArgumentException">If the result does not fit in <see cref="UInt64"/></exception>
    public static MicroCCDAmount operator -(MicroCCDAmount a, MicroCCDAmount b)
    {
        try
        {
            UInt64 newAmount = checked(a.GetMicroCcdValue() - b.GetMicroCcdValue());
            return MicroCCDAmount.FromCcd(newAmount);
        }
        catch (OverflowException)
        {
            throw new ArgumentException(
                $"The result of {a.GetMicroCcdValue()} + {b.GetMicroCcdValue()} does not fit in UInt64."
            );
        }
    }

    public bool Equals(MicroCCDAmount other)
    {
        return _microCcd == other._microCcd;
    }

    public override bool Equals(Object? obj)
    {
        return obj is MicroCCDAmount other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _microCcd.GetHashCode();
    }
}
