namespace Concordium.Sdk.Types;

/// <summary>
/// Smart contract schema type.
/// This represents a single type as part of a smart contract module schema, and allows for
/// converting structure data, such as JSON, from and to the binary representation used by a
/// smart contract.
/// </summary>
public sealed record SchemaType(byte[] Type) : IEquatable<SchemaType>
{
    /// <summary>Construct SchemaType from a HEX encoding.</summary>
    public static SchemaType FromHexString(string hexString)
    {
        var value = Convert.FromHexString(hexString);
        return new(value);
    }

    /// <summary>Construct SchemaType from a base64 encoding.</summary>
    public static SchemaType FromBase64String(string base64)
    {
        var value = Convert.FromBase64String(base64);
        return new(value);
    }

    /// <summary>Check for equality.</summary>
    public bool Equals(SchemaType? other) => other != null && this.Type.SequenceEqual(other.Type);

    /// <summary>Gets hash code.</summary>
    public override int GetHashCode()
    {
        var paramHash = Helpers.HashCode.GetHashCodeByteArray(this.Type);
        return paramHash;
    }

    /// <summary>
    /// Convert schema type to hex string.
    /// </summary>
    public string ToHexString() => Convert.ToHexString(this.Type).ToLowerInvariant();
}
