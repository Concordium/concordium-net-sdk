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
        {
            throw new ArgumentException(
                $"The provided hex string must be {BytesLength * 2} characters long."
            );
        }

        try
        {
            this._value = Convert.FromHexString(hashAsBase16String);
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
        {
            throw new ArgumentException(
                $"The provided byte array must be {BytesLength} bytes long."
            );
        }

        this._value = (byte[])hashAsBytes.Clone();
    }

    /// <summary>
    /// Get the hash as a length-32 byte array.
    /// </summary>
    public byte[] GetBytes() => (byte[])this._value.Clone();

    /// <summary>
    /// Get the hash as a length-64 hex encoded string.
    /// </summary>
    public sealed override string ToString() => Convert.ToHexString(this._value).ToLowerInvariant();

    public virtual bool Equals(Hash? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other.GetType() != this.GetType())
        {
            return false;
        }

        return this._value.SequenceEqual(other._value);
    }

    public override int GetHashCode() => this._value.GetHashCode();
}
