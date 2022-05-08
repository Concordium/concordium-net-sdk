namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents the nonce see <a href="https://developer.concordium.software/en/mainnet/net/resources/glossary.html?highlight=nonce#nonce">here</a>).
/// </summary>
public readonly struct Nonce
{
    private readonly ulong _value;

    /// <summary>
    /// Creates an instance from a ulong representing nonce.
    /// </summary>
    /// <param name="value">nonce as ulong.</param>
    public Nonce(ulong value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the nonce as ulong.
    /// </summary>
    public ulong AsUInt64 => _value;

    /// <summary>
    /// Increments nonce value by 1.
    /// </summary>
    /// <returns></returns>
    public Nonce Increment()
    {
        return new Nonce(_value + 1);
    }

    public bool Equals(Nonce other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Nonce other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(Nonce left, Nonce right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Nonce left, Nonce right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}
