namespace ConcordiumNetSdk.Types;

/// <summary>
/// Models an account credential index.
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

    public static implicit operator AccountCredentialIndex(byte value)
    {
        return new AccountCredentialIndex(value);
    }

    public static implicit operator byte(AccountCredentialIndex accountCredentialIndex)
    {
        return accountCredentialIndex.Value;
    }
}
