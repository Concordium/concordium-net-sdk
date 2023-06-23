namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an account credential index.
///
/// An account has one or more credentials, each identified by a unique
/// byte-value referred to as its credential index.
/// </summary>
/// <param name="Value">An account credential index represented by a <see cref="byte"/>.</param>
public readonly record struct AccountCredentialIndex(byte Value)
{
    /// <summary>
    /// Creates an instance from a string representing a <see cref="byte"/> value.
    /// </summary>
    /// <param name="index">An index represented as a string representing to be parsed as a <see cref="byte"/> value.</param>
    /// <exception cref="ArgumentException">The index could not be parsed as a <see cref="byte"/> value.</exception>
    public static AccountCredentialIndex From(string index)
    {
        if (byte.TryParse(index, out var result))
        {
            return new AccountCredentialIndex(result);
        }
        throw new ArgumentException($"Could not parse '{index}' as a byte value.");
    }
}
