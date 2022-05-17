namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base16 encoded hash.
/// </summary>
public abstract class Hash
{
    private const int StringLength = 64;
    public const int BytesLength = 32;

    private readonly string _formatted;
    private readonly byte[] _value;

    /// <summary>
    /// Creates an instance from a base16 encoded string representing hash (64 characters).
    /// </summary>
    /// <param name="hashAsBase16String">the hash as base16 encoded string.</param>
    protected Hash(string hashAsBase16String)
    {
        if (hashAsBase16String.Length != StringLength) throw new ArgumentException($"The hash base16 encoded string length must be {StringLength}.");
        _value = Convert.FromHexString(hashAsBase16String);
        _formatted = hashAsBase16String.ToLowerInvariant();
    }

    /// <summary>
    /// Creates an instance from a 32 bytes representing hash.
    /// </summary>
    /// <param name="hashAsBytes">the hash as 32 bytes.</param>
    protected Hash(byte[] hashAsBytes)
    {
        if (hashAsBytes.Length != BytesLength) throw new ArgumentException($"The hash bytes length must be {BytesLength}.");
        _value = hashAsBytes;
        _formatted = Convert.ToHexString(hashAsBytes).ToLowerInvariant();
    }

    /// <summary>
    /// Gets the hash as a base16 encoded string.
    /// </summary>
    public string AsString => _formatted;

    /// <summary>
    /// Gets the hash as a 32 byte array.
    /// </summary>
    public byte[] AsBytes => _value;

    public override string ToString()
    {
        return _formatted;
    }

    public override bool Equals(object? obj)
    {
        return obj is Hash other
               && GetType() == obj.GetType()
               && _formatted == other._formatted;
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
