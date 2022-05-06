namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents a base64 encoded salt.
/// </summary>
public record Salt
{
    private const int StringLength = 24;
    private const int BytesLength = 16;

    private readonly string _formatted;
    private readonly byte[] _value;

    /// <summary>
    /// Creates an instance from a base64 encoded string salt.
    /// </summary>
    /// <param name="saltAsBase64String">salt as base64 encoded string (24 characters).</param>
    public Salt(string saltAsBase64String)
    {
        if (saltAsBase64String.Length != StringLength) throw new ArgumentException($"The salt base64 encoded string length must be {StringLength}.");
        _value = Convert.FromBase64String(saltAsBase64String);
        _formatted = saltAsBase64String;
    }
    
    /// <summary>
    /// Creates an instance from a 16 byte salt.
    /// </summary>
    /// <param name="saltAsBytes">salt as 16 bytes.</param>
    public Salt(byte[] saltAsBytes)
    {
        if (saltAsBytes.Length != BytesLength) throw new ArgumentException($"The salt bytes length must be {BytesLength}.");
        _formatted = Convert.ToBase64String(saltAsBytes);
        _value = saltAsBytes;
    }

    /// <summary>
    /// Gets the salt as a base64 encoded string.
    /// </summary>
    public string AsString => _formatted;

    /// <summary>
    /// Gets the salt as a 16 byte array.
    /// </summary>
    public byte[] AsBytes => _value;
    
    public override string ToString() => AsString;
}
