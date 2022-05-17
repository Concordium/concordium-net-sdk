namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents a base64 encoded cipher text.
/// </summary>
public record CipherText
{
    private const int StringLength = 108;
    private const int BytesLength = 80;

    private readonly string _formatted;
    private readonly byte[] _value;

    private CipherText(string cipherTextAsBase64String)
    {
        _value = Convert.FromBase64String(cipherTextAsBase64String);
        _formatted = cipherTextAsBase64String;
    }
    
    private CipherText(byte[] cipherTextAsBytes)
    {
        _formatted = Convert.ToBase64String(cipherTextAsBytes);
        _value = cipherTextAsBytes;
    }

    /// <summary>
    /// Gets the cipher text as a base64 encoded string.
    /// </summary>
    public string AsString => _formatted;

    /// <summary>
    /// Gets the cipher text as a 80 byte array.
    /// </summary>
    public byte[] AsBytes => _value;

    /// <summary>
    /// Creates an instance from a base64 encoded string cipher text.
    /// </summary>
    /// <param name="cipherTextAsBase64String">the cipher text as base64 encoded string (108 characters).</param>
    public static CipherText From(string cipherTextAsBase64String)
    {
        if (cipherTextAsBase64String.Length != StringLength) throw new ArgumentException($"The cipher text base64 encoded string length must be {StringLength}.");
        return new CipherText(cipherTextAsBase64String);
    }

    /// <summary>
    /// Creates an instance from a 80 byte cipher text.
    /// </summary>
    /// <param name="cipherTextAsBytes">the cipher text as 80 bytes.</param>
    public static CipherText From(byte[] cipherTextAsBytes)
    {
        if (cipherTextAsBytes.Length != BytesLength) throw new ArgumentException($"The cipher text bytes length must be {BytesLength}.");
        return new CipherText(cipherTextAsBytes);
    }

    public override string ToString() => AsString;
}
