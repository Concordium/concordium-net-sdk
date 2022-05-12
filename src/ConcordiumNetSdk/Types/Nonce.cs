using System.Buffers.Binary;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents the nonce see <a href="https://developer.concordium.software/en/mainnet/net/resources/glossary.html?highlight=nonce#nonce">here</a>).
/// </summary>
public readonly struct Nonce
{
    public const int BytesLength = 8;

    private readonly ulong _value;

    private Nonce(ulong nonce)
    {
        _value = nonce;
    }

    /// <summary>
    /// Gets the nonce as ulong.
    /// </summary>
    public ulong AsUInt64 => _value;

    /// <summary>
    /// Creates an instance from a ulong representing nonce.
    /// </summary>
    /// <param name="nonce">nonce as ulong.</param>
    public static Nonce Create(ulong nonce)
    {
        return new Nonce(nonce);
    }

    /// <summary>
    /// Increments nonce value by 1.
    /// </summary>
    /// <returns></returns>
    public Nonce Increment()
    {
        return new Nonce(_value + 1);
    }

    /// <summary>
    /// Serializes nonce to byte format.
    /// </summary>
    /// <returns><see cref="T:byte[]"/> - serialized nonce in byte format.</returns>
    public byte[] SerializeToBytes()
    {
        var bytes = new byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(new Span<byte>(bytes), _value);
        return bytes;
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
