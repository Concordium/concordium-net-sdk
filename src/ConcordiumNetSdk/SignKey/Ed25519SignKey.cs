using NSec.Cryptography;

namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// An ed25519 sign key.
/// </summary>
public record Ed25519SignKey : ISigner
{
    public const int SignKeyBytesLength = 32;
    public const int SignatureBytesLength = 64;

    /// <summary>
    /// Representation of the ed25519 sign key as a length-64 byte array.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsBytes">A byte array representing the sign key.</param>
    private Ed25519SignKey(byte[] signKeyAsBytes)
    {
        _value = signKeyAsBytes;
    }

    /// <summary>
    /// Get the ed25519 sign key as a length-64 hex encoded string.
    /// </summary>
    public override string ToString()
    {
        return Convert.ToHexString(_value).ToLowerInvariant();
    }

    /// <summary>
    /// Get the ed25519 sign key as a length-32 byte array.
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
        byte[] bytes = Convert.FromHexString(signKeyAsHexString);
        if (signKeyAsHexString.Length != SignKeyBytesLength * 2)
            throw new ArgumentException(
                $"The sign key hex encoded string must be {SignKeyBytesLength * 2} characters."
            );
        return new Ed25519SignKey(bytes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ed25519SignKey"/> class.
    /// </summary>
    /// <param name="signKeyAsBytes">A byte array representing the sign key.</param>
    public static Ed25519SignKey From(byte[] signKeyAsBytes)
    {
        if (signKeyAsBytes.Length != SignKeyBytesLength)
            throw new ArgumentException($"The sign key array must be {SignKeyBytesLength} bytes.");
        return new Ed25519SignKey(signKeyAsBytes);
    }

    public UInt32 GetSignatureLength()
    {
        return SignatureBytesLength;
    }

    /// <summary>
    /// Signs the provided data using the ed25519 sign key.
    /// </summary>
    /// <param name="bytes">A byte array representing the data to sign.</param>
    /// <return>A byte array representing the signature.</param>
    public byte[] Sign(byte[] bytes)
    {
        Ed25519 algorithm = SignatureAlgorithm.Ed25519;
        using Key key = Key.Import(algorithm, _value, KeyBlobFormat.RawPrivateKey);
        return algorithm.Sign(key, bytes);
    }
}
