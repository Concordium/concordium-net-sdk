using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Models an account sequence number.
///
/// The account sequence number is a 64-bit integer specific to an account and used
/// when submitting account transactions for that account to the node. The account
/// sequence number is maintained as on-chain state and is incremented with each finalized
/// transaction. The next sequence number to be used in a transaction can be queried
/// with <see cref="Client.GetNextAccountSequenceNumber"> or
/// <see cref="Client.GetAccountInfo"/>.
/// </summary>
public readonly struct AccountNonce : IEquatable<AccountNonce>
{
    public const UInt32 BytesLength = sizeof(UInt64);

    /// <summary>
    /// The value of the nonce.
    /// </summary>
    public readonly UInt64 Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountNonce"/> class.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    public AccountNonce(UInt64 nonce)
    {
        Value = nonce;
    }

    /// <summary>
    /// Returns a new nonce whose value is increased by 1 relative to the current nonce.
    /// </summary>
    public AccountNonce GetIncrementedNonce()
    {
        return new AccountNonce(Value + 1);
    }

    /// <summary>
    /// Get the account nonce in the binary format expected by the node.
    /// </summary>
    public byte[] GetBytes()
    {
        return Serialization.GetBytes((UInt64)Value);
    }

    /// <summary>
    /// Converts the account nonce to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.SequenceNumber ToProto()
    {
        return new Concordium.V2.SequenceNumber() { Value = Value };
    }

    public static implicit operator AccountNonce(UInt64 value)
    {
        return new AccountNonce(value);
    }

    public static implicit operator UInt64(AccountNonce byteIndex)
    {
        return byteIndex.Value;
    }

    public bool Equals(AccountNonce other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is AccountNonce other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
