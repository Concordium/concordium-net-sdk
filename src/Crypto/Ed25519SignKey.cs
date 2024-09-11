using NSec.Cryptography;

namespace Concordium.Sdk.Crypto;

/// <summary>
/// Represents a (secret) sign key of an ed25519 keypair.
///
/// Used for signing transactions.
///
/// Equality are by comparing elements of underlying byte array.
/// </summary>
public sealed record Ed25519SignKey : ISigner, IEquatable<Ed25519SignKey>
{
    /// <summary>
    /// The length of an ed25519 (secret) sign key in bytes.
    /// </summary>
    public const int SignKeyBytesLength = 32;

    /// <summary>
    /// Representation of the ed25519 (secret) sign key as a length-32 byte array.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsBytes">A length-32 byte array representing the ed25519 (secret) sign key.</param>
    private Ed25519SignKey(byte[] signKeyAsBytes) => this._value = signKeyAsBytes;

    /// <summary>
    /// Gets the ed25519 (secret) sign key represented as a length-64 hex encoded string.
    /// </summary>
    public override string ToString() => Convert.ToHexString(this._value).ToLowerInvariant();

    /// <summary>
    /// Copies the ed25519 (secret) sign to length-32 byte array.
    /// </summary>
    public byte[] ToBytes() => this._value.ToArray();

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsHexString">A length-64 hex encoded string representing the sign key.</param>
    /// <exception cref="ArgumentException">The sign key was not a length-64 hex encoded string.</exception>
    public static Ed25519SignKey From(string signKeyAsHexString)
    {
        byte[] bytes;
        try
        {
            bytes = Convert.FromHexString(signKeyAsHexString);
        }
        catch (FormatException e)
        {
            throw new ArgumentException($"'{signKeyAsHexString}' is not a valid hex formatted string.", e);
        }
        return From(bytes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsBytes">A length-32 byte array representing the ed25519 (secret) sign key.</param>
    /// <exception cref="ArgumentException">The sign key was not a length-32 byte array.</exception>
    public static Ed25519SignKey From(byte[] signKeyAsBytes)
    {
        if (signKeyAsBytes.Length != SignKeyBytesLength)
        {
            throw new ArgumentException($"The sign key must correspond to a key that is {SignKeyBytesLength} bytes.");
        }

        return new Ed25519SignKey(signKeyAsBytes);
    }

    /// <summary>
    /// Signs the provided data using the ed25519 (secret) sign key.
    /// </summary>
    /// <param name="bytes">A byte array representing the data to sign.</param>
    /// <return>A length-64 byte array representing the signature.</return>
    public byte[] Sign(byte[] bytes)
    {
        var algorithm = SignatureAlgorithm.Ed25519;
        using var key = Key.Import(algorithm, this._value, KeyBlobFormat.RawPrivateKey);
        return algorithm.Sign(key, bytes);
    }

    /// <summary> Determines whether one Ed25519SignKey is equal to a second Ed25519SignKey. </summary>
    public bool Equals(Ed25519SignKey? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this._value.SequenceEqual(other._value);
    }

    /// <summary> Get hash code for the Ed25519SignKey. </summary>
    public override int GetHashCode() => Helpers.HashCode.GetHashCodeByteArray(this._value);
}
