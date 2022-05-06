namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents a base-64 encoded cipher text.
/// </summary>
public record CipherText
{
    private const int StringLength = 108;
    private const int BytesLength = 80;

    private readonly string _formatted;
    private readonly byte[] _value;

    /// <summary>
    /// Creates an instance from a base-64 encoded string cipher text.
    /// </summary>
    /// <param name="cipherTextAsBase64String">cipher text as base64 encoded string (108 characters).</param>
    public CipherText(string cipherTextAsBase64String)
    {
        if (cipherTextAsBase64String.Length != StringLength) throw new ArgumentException($"The cipher text base64 encoded string length must be {StringLength}.");
        _value = Convert.FromBase64String(cipherTextAsBase64String);
        _formatted = cipherTextAsBase64String;
    }
    
    /// <summary>
    /// Creates an instance from a 80 byte cipher text.
    /// </summary>
    /// <param name="cipherTextAsBytes">cipher text as 80 bytes.</param>
    public CipherText(byte[] cipherTextAsBytes)
    {
        if (cipherTextAsBytes.Length != BytesLength) throw new ArgumentException($"The cipher text bytes length must be {BytesLength}.");
        _formatted = Convert.ToBase64String(cipherTextAsBytes);
        _value = cipherTextAsBytes;
    }

    /// <summary>
    /// Gets the cipher text as a base-64 encoded string.
    /// </summary>
    public string AsString => _formatted;

    /// <summary>
    /// Gets the cipher text as a 80 byte array.
    /// </summary>
    public byte[] AsBytes => _value;

    public override string ToString() => AsString;
}
