namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a 32-byte hash.
/// </summary>
public abstract record Hash : IEquatable<Hash>
{
    public const int BytesLength = 32;

    /// <summary>
    /// A length-32 byte array representing a hash.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    /// <param name="hashAsBase16String">A hash represented as a length-64 hex encoded string.</param>
    /// <exception cref="ArgumentException">The supplied string is not a 64-character hex encoded string.</exception>
    protected Hash(string hashAsBase16String)
    {
        if (hashAsBase16String.Length != BytesLength * 2)
            throw new ArgumentException(
                $"The provided hex string must be {BytesLength * 2} characters long."
            );
        try
        {
            _value = Convert.FromHexString(hashAsBase16String);
        }
        catch (FormatException)
        {
            throw new ArgumentException("The provided string must be hex encoded.");
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    /// <param name="hashAsBytes">A hash represented as a length-32 byte array.</param>
    /// <exception cref="FormatException">The supplied array is not 32 bytes long.</exception>
    protected Hash(byte[] hashAsBytes)
    {
        if (hashAsBytes.Length != BytesLength)
            throw new ArgumentException(
                $"The provided byte array must be {BytesLength} bytes long."
            );
        _value = (byte[])hashAsBytes.Clone();
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
    public sealed override string ToString()
    {
        return Convert.ToHexString(_value).ToLowerInvariant();
    }

    public virtual bool Equals(Hash? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        var other = (Hash)obj;
        return _value.SequenceEqual(other._value);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}
