namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base-16 encoded hash (64 characters).
/// </summary>
public abstract class Hash
{
    private readonly string _formatted;
    private readonly byte[] _value;

    /// <summary>
    /// Creates an instance from a 32 byte hash.
    /// </summary>
    /// <param name="bytes">32 byte hash</param>
    protected Hash(byte[] bytes)
    {
        if (bytes.Length != 32) throw new ArgumentException("value must be 32 bytes");
        _value = bytes;
        _formatted = Convert.ToHexString(bytes).ToLowerInvariant();
    }

    /// <summary>
    /// Creates an instance from a base-16 encoded string hash.
    /// </summary>
    /// <param name="base16EncodedHash">base-16 encoded hash.</param>
    protected Hash(string base16EncodedHash)
    {
        if (base16EncodedHash.Length != 64) throw new ArgumentException("string must be 64 char hex string.");
        _value = Convert.FromHexString(base16EncodedHash);
        _formatted = base16EncodedHash.ToLowerInvariant();
    }

    /// <summary>
    /// Gets the hash as a base-16 encoded string.
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
