using System.Buffers.Binary;

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
    private const UInt64 _oneMillion = 1_000_000;

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
    /// Gets µCCD amount.
    /// </summary>
    public UInt64 GetMicroCcdValue()
    {
        return _microCcd;
    }

    /// <summary>
    /// Gets a formatted string representing the amount in µCCD.
    /// </summary>
    public string GetFormattedMicroCcd()
    {
        return $"{_microCcd}";
    }

    /// <summary>
    /// Gets a formatted string representing the amount in CCD.
    /// </summary>
    public string GetFormattedCcd()
    {
        return $"{_microCcd / (decimal)_oneMillion}";
    }

    /// <summary>
    /// Creates an instance from a µCCD amount.
    /// </summary>
    /// <param name="microCcd">The amount in µCCD.</param>
    public static MicroCCDAmount FromMicroCcd(UInt64 microCcd)
    {
        return new MicroCCDAmount(microCcd);
    }

    /// <summary>
    /// Creates an instance from a CCD value.
    /// </summary>
    /// <param name="ccd">the CCD value.</param>
    public static MicroCCDAmount FromCcd(UInt64 ccd)
    {
        return new MicroCCDAmount(ccd * _oneMillion);
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
