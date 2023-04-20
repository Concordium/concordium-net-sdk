using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// An account nonce.
/// </summary>
public class AccountNonce : IEquatable<AccountNonce>
{
    /// <summary>
    /// The value of the nonce.
    /// </summary>
    private readonly UInt64 _value;
    public const int BytesLength = 8;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountNonce"/> class.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    private AccountNonce(UInt64 nonce)
    {
        _value = nonce;
    }

    /// <summary>
    /// Get the nonce.
    /// </summary>
    public ulong GetValue()
    {
        return _value;
    }

    /// <summary>
    /// Creates a nonce instance.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    public static AccountNonce Create(UInt64 nonce)
    {
        return new AccountNonce(nonce);
    }

    /// <summary>
    /// Returns a new nonce whose value is increased by 1 relative to the current nonce.
    /// </summary>
    public AccountNonce GetIncrementedNonce()
    {
        return new AccountNonce(_value + 1);
    }

    /// <summary>
    /// Get the address as a length-32 byte array without the version byte prepended.
    /// </summary>
    public byte[] GetBytes()
    {
        return Serialization.GetBytes((UInt64)_value);
    }

    /// <summary>
    /// Converts the account nonce to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.SequenceNumber ToProto()
    {
        return new Concordium.V2.SequenceNumber() { Value = _value };
    }

    public bool Equals(AccountNonce? other)
    {
        return other is not null && _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is AccountNonce other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}
