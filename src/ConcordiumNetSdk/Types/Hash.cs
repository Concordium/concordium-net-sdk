namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a hash.
/// </summary>
public abstract class Hash
{
    private const int StringLength = 64;
    public const int BytesLength = 32;

    /// <summary>
    /// A lowercase length-64 hex encoded string representing the hash.
    /// </summary>
    private readonly string _formatted;

    /// <summary>
    /// A length-32 byte array representing a hash.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    /// <param name="hashAsBase16String">A hash represented as a length-64 hex encoded string.</param>
    protected Hash(string hashAsBase16String)
    {
        if (hashAsBase16String.Length != StringLength)
            throw new ArgumentException(
                $"The provided hex string must be {StringLength} characters long."
            );
        _value = Convert.FromHexString(hashAsBase16String);
        _formatted = hashAsBase16String.ToLowerInvariant();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    /// <param name="hashAsBytes">A hash represented as a length-32 byte array.</param>
    protected Hash(byte[] hashAsBytes)
    {
        if (hashAsBytes.Length != BytesLength)
            throw new ArgumentException(
                $"The provided byte array must be {BytesLength} bytes long."
            );
        _value = hashAsBytes;
        _formatted = Convert.ToHexString(hashAsBytes).ToLowerInvariant();
    }

    /// <summary>
    /// Get the hash as a length-32 byte array.
    /// </summary>
    public byte[] GetBytes()
    {
        return (byte[])_value.Clone();
    }

    /// <summary>
    /// Get the hash as a length-64 hex encoded string.
    /// </summary>
    public override string ToString()
    {
        return (string)_formatted.Clone();
    }

    public override bool Equals(object? obj)
    {
        return obj is Hash other && GetType() == obj.GetType() && _formatted == other._formatted;
    }

    public override int GetHashCode()
    {
        return _formatted.GetHashCode();
    }

    public static bool operator ==(Hash? left, Hash? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Hash? left, Hash? right)
    {
        return !Equals(left, right);
    }
}
