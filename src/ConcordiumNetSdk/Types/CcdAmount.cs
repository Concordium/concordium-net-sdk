using System.Buffers.Binary;

namespace ConcordiumNetSdk.Types;

// todo: implement tests
/// <summary>
/// Represents an amount of microCCD.
/// </summary>
public readonly struct CcdAmount
{
    private readonly ulong _microCcd;

    private CcdAmount(ulong microCcd)
    {
        _microCcd = microCcd;
    }

    /// <summary>
    /// Creates an zero microCCD amount.
    /// </summary>
    public static CcdAmount Zero => new(0);

    /// <summary>
    /// Gets the microCCD amount value.
    /// </summary>
    public ulong MicroCcdValue => _microCcd;

    /// <summary>
    /// Gets the formatted microCCD.
    /// </summary>
    public string FormattedMicroCcd => $"{_microCcd}";

    /// <summary>
    /// Gets the formatted CCD.
    /// </summary>
    public string FormattedCcd => $"{_microCcd / (decimal) 1000000}";

    /// <summary>
    /// Creates an instance from microCCD value.
    /// </summary>
    /// <param name="microCcd">the microCCD value.</param>
    public static CcdAmount FromMicroCcd(ulong microCcd)
    {
        return new CcdAmount(microCcd);
    }

    /// <summary>
    /// Creates an instance from microCCD value.
    /// </summary>
    /// <param name="microCcd">the microCCD value.</param>
    public static CcdAmount FromMicroCcd(int microCcd)
    {
        if (microCcd < 0) throw new ArgumentOutOfRangeException(nameof(microCcd), "Cannot represent negative numbers.");
        return new CcdAmount(Convert.ToUInt64(microCcd));
    }

    /// <summary>
    /// Creates an instance from CCD value.
    /// </summary>
    /// <param name="ccd">the CCD value.</param>
    public static CcdAmount FromCcd(int ccd)
    {
        if (ccd < 0) throw new ArgumentOutOfRangeException(nameof(ccd), "Cannot represent negative numbers.");
        return new CcdAmount((ulong) ccd * 1_000_000);
    }

    /// <summary>
    /// Serializes microCCD amount to byte format.
    /// </summary>
    /// <returns><see cref="T:byte[]"/> - serialized microCCD amount in byte format.</returns>
    public byte[] SerializeToBytes()
    {
        var bytes = new byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(new Span<byte>(bytes), _microCcd);
        return bytes;
    }

    public static CcdAmount operator +(CcdAmount a, CcdAmount b)
    {
        return new(a._microCcd + b._microCcd);
    }

    public static CcdAmount operator *(CcdAmount a, int b)
    {
        var result = a._microCcd * Convert.ToUInt32(b);
        return new(result);
    }

    public static CcdAmount operator *(int a, CcdAmount b)
    {
        return b * a;
    }

    public bool Equals(CcdAmount other)
    {
        return _microCcd == other._microCcd;
    }

    public override bool Equals(object? obj)
    {
        return obj is CcdAmount other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _microCcd.GetHashCode();
    }

    public static bool operator ==(CcdAmount left, CcdAmount right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CcdAmount left, CcdAmount right)
    {
        return !left.Equals(right);
    }

    public static bool operator >(CcdAmount left, CcdAmount right)
    {
        return left._microCcd > right._microCcd;
    }

    public static bool operator >=(CcdAmount left, CcdAmount right)
    {
        return left._microCcd >= right._microCcd;
    }

    public static bool operator <(CcdAmount left, CcdAmount right)
    {
        return left._microCcd < right._microCcd;
    }

    public static bool operator <=(CcdAmount left, CcdAmount right)
    {
        return left._microCcd <= right._microCcd;
    }

    public override string ToString()
    {
        return $"{_microCcd} µCCD";
    }
}
