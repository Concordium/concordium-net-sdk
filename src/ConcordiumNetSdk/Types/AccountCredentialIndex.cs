namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents an account credential index.
///
/// An account has one or more credentials, each identified by a unique
/// byte-value referred to as its credential index.
/// </summary>
public readonly struct AccountCredentialIndex
{
    public readonly byte Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountCredentialIndex"/> class.
    /// </summary>
    /// <param name="value">An account credential index represented by a <see cref="byte"/>.</param>
    public AccountCredentialIndex(byte value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an instance from a string representing a <see cref="byte"/> value.
    /// </summary>
    /// <param name="index">An index represented as a string representing to be parsed as a <see cref="byte"/> value.</param>
    public static AccountCredentialIndex From(string index)
    {
        byte result;
        if (Byte.TryParse(index, out result))
        {
            return new AccountCredentialIndex(result);
        }
        throw new ArgumentException("Could not parse the account credential index.");
    }

    public static implicit operator AccountCredentialIndex(byte value)
    {
        return new AccountCredentialIndex(value);
    }

    public static implicit operator byte(AccountCredentialIndex accountCredentialIndex)
    {
        return accountCredentialIndex.Value;
    }
}
