using NSec.Cryptography;

namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents a hex encoded ed25519 sign key.
/// </summary>
public record Ed25519SignKey : ISigner
{
    private const int StringLength = 64;
    private const int BytesLength = 32;

    private readonly string _formatted;
    private readonly byte[] _value;

    private Ed25519SignKey(string signKeyAsHexString)
    {
        _value = Convert.FromHexString(signKeyAsHexString);
        _formatted = signKeyAsHexString;
    }

    private Ed25519SignKey(byte[] signKeyAsBytes)
    {
        _formatted = Convert.ToHexString(signKeyAsBytes).ToLowerInvariant();
        _value = signKeyAsBytes;
    }

    /// <summary>
    /// Gets the ed25519 sign key as a hex encoded string.
    /// </summary>
    public string AsString => _formatted;

    /// <summary>
    /// Gets the ed25519 sign key as a 64 byte array.
    /// </summary>
    public byte[] AsBytes => _value;

    /// <summary>
    /// Creates an instance from a hex encoded string ed25519 representing sign key.
    /// </summary>
    /// <param name="signKeyAsHexString">the sign key as hex encoded string (64 characters).</param>
    public static Ed25519SignKey From(string signKeyAsHexString)
    {
        if (signKeyAsHexString.Length != StringLength) throw new ArgumentException($"The sign key hex encoded string length must be {StringLength}.");
        return new Ed25519SignKey(signKeyAsHexString);
    }

    /// <summary>
    /// Creates an instance from a 32 byte ed25519 representing sign key.
    /// </summary>
    /// <param name="signKeyAsBytes">the sign key as 32 bytes.</param>
    public static Ed25519SignKey From(byte[] signKeyAsBytes)
    {
        if (signKeyAsBytes.Length != BytesLength) throw new ArgumentException($"The sign key bytes length must be {BytesLength}.");
        return new Ed25519SignKey(signKeyAsBytes);
    }

    public byte[] Sign(byte[] bytes)
    {
        Ed25519 algorithm = SignatureAlgorithm.Ed25519;
        using Key key = Key.Import(algorithm, _value, KeyBlobFormat.RawPrivateKey);
        return algorithm.Sign(key, bytes);
    }

    public override string ToString() => AsString;
}
