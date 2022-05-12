namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents a base64 encoded initialization vector.
/// </summary>
public record InitializationVector
{
    private const int StringLength = 24;
    private const int BytesLength = 16;

    private readonly string _formatted;
    private readonly byte[] _value;

    private InitializationVector(string initializationVectorAsBase64String)
    {
        _value = Convert.FromBase64String(initializationVectorAsBase64String);
        _formatted = initializationVectorAsBase64String;
    }
    
    private InitializationVector(byte[] initializationVectorAsBytes)
    {
        _formatted = Convert.ToBase64String(initializationVectorAsBytes);
        _value = initializationVectorAsBytes;
    }
    
    /// <summary>
    /// Gets the initialization vector as a base64 encoded string.
    /// </summary>
    public string AsString => _formatted;

    /// <summary>
    /// Gets the initialization vector as a 16 byte array.
    /// </summary>
    public byte[] AsBytes => _value;

    /// <summary>
    /// Creates an instance from a base64 encoded string initialization vector.
    /// </summary>
    /// <param name="initializationVectorAsBase64String">the initialization vector as base64 encoded string (24 characters).</param>
    public static InitializationVector From(string initializationVectorAsBase64String)
    {
        if (initializationVectorAsBase64String.Length != StringLength) throw new ArgumentException($"The initialization vector base64 encoded string length must be {StringLength}.");
        return new InitializationVector(initializationVectorAsBase64String);
    }

    /// <summary>
    /// Creates an instance from a 16 byte initialization vector.
    /// </summary>
    /// <param name="initializationVectorAsBytes">the initialization vector as 16 bytes.</param>
    public static InitializationVector From(byte[] initializationVectorAsBytes)
    {
        if (initializationVectorAsBytes.Length != BytesLength) throw new ArgumentException($"The initialization vector bytes length must be {BytesLength}.");
        return new InitializationVector(initializationVectorAsBytes);
    }

    public override string ToString() => _formatted;
}
