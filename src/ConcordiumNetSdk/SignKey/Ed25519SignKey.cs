using NSec.Cryptography;

namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// An ed25519 sign key.
/// </summary>
public record Ed25519SignKey : ISigner
{
    private const int StringLength = 64;
    private const int BytesLength = 32;

    /// <summary>
    /// Representation of the ed25519 sign key as a length-32 hex string.
    /// </summary>
    private readonly string _formatted;

    /// <summary>
    /// Representation of the ed25519 sign key as a length-64 byte array.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsHexString">A hex encoded string representing the sign key.</param>
    private Ed25519SignKey(string signKeyAsHexString)
    {
        _value = Convert.FromHexString(signKeyAsHexString);
        _formatted = signKeyAsHexString;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsBytes">A byte array representing the sign key.</param>
    private Ed25519SignKey(byte[] signKeyAsBytes)
    {
        _formatted = Convert.ToHexString(signKeyAsBytes).ToLowerInvariant();
        _value = signKeyAsBytes;
    }

    /// <summary>
    /// Get the ed25519 sign key as a length-32 hex encoded string.
    /// </summary>
    public string GetString()
    {
        return (string)_formatted.Clone();
    }

    /// <summary>
    /// Get the ed25519 sign key as a length-64 byte array.
    /// </summary>
    public byte[] GetBytes()
    {
        return (byte[])_value.Clone();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsHexString">A hex encoded string representing the sign key.</param>
    public static Ed25519SignKey From(string signKeyAsHexString)
    {
        if (signKeyAsHexString.Length != StringLength)
            throw new ArgumentException(
                $"The sign key hex encoded string length must be {StringLength}."
            );
        return new Ed25519SignKey(signKeyAsHexString);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsBytes">A byte array representing the sign key.</param>
    public static Ed25519SignKey From(byte[] signKeyAsBytes)
    {
        if (signKeyAsBytes.Length != BytesLength)
            throw new ArgumentException($"The sign key bytes length must be {BytesLength}.");
        return new Ed25519SignKey(signKeyAsBytes);
    }

    /// <summary>
    /// Signs the provided data using the ed25519 sign key.
    /// </summary>
    /// <param name="bytes">A byte array containing the data to sign.</param>
    public byte[] Sign(byte[] bytes)
    {
        Ed25519 algorithm = SignatureAlgorithm.Ed25519;
        using Key key = Key.Import(algorithm, _value, KeyBlobFormat.RawPrivateKey);
        return algorithm.Sign(key, bytes);
    }
}
