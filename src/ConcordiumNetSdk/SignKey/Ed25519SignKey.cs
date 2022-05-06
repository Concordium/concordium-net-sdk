namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents a hex encoded ed25519 sign key.
/// </summary>
public record Ed25519SignKey
{
    private const int StringLength = 64;
    private const int BytesLength = 64;

    private readonly string _formatted;
    private readonly byte[] _value;

    /// <summary>
    /// Creates an instance from a hex encoded string ed25519 sign key.
    /// </summary>
    /// <param name="signKeyAsHexString">sign key as hex encoded string (64 characters).</param>
    public Ed25519SignKey(string signKeyAsHexString)
    {
        if (signKeyAsHexString.Length != StringLength) throw new ArgumentException($"The sign key hex encoded string length must be {StringLength}.");
        _value = Convert.FromHexString(signKeyAsHexString);
        _formatted = signKeyAsHexString;
    }
    
    /// <summary>
    /// Creates an instance from a 64 byte ed25519 sign key.
    /// </summary>
    /// <param name="signKeyAsBytes">sign key as 64 bytes.</param>
    public Ed25519SignKey(byte[] signKeyAsBytes)
    {
        if (signKeyAsBytes.Length != BytesLength) throw new ArgumentException($"The sign key bytes length must be {BytesLength}.");
        _formatted = Convert.ToHexString(signKeyAsBytes);
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
    
    public override string ToString() => AsString;
}
