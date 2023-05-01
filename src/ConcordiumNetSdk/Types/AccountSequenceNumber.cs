using ConcordiumNetSdk.Client;
using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents an account sequence number.
///
/// The account sequence number is a 64-bit unsigned integer specific to an account
/// and used when submitting account transactions for that account to the node. The account
/// sequence number is maintained as on-chain state and is incremented with each finalized
/// transaction. The next sequence number to be used in a transaction can be queried
/// with <see cref="ConcordiumClient.GetNextAccountSequenceNumber"> or
/// <see cref="ConcordiumClient.GetAccountInfo"/>.
/// </summary>
public readonly struct AccountSequenceNumber : IEquatable<AccountSequenceNumber>
{
    public const UInt32 BytesLength = sizeof(UInt64);

    /// <summary>
    /// The value of the account sequence number.
    /// </summary>
    public readonly UInt64 Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountSequenceNumber"/> class.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    private AccountSequenceNumber(UInt64 nonce)
    {
        Value = nonce;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AccountSequenceNumber"/> class from an integer.
    /// </summary>
    /// <param name="nonce">The account sequence number represented as an integer.</param>
    /// <exception cref="ArgumentException">The supplied account sequence number is 0.</exception>
    public AccountSequenceNumber From(UInt64 sequenceNumber)
    {
        if (sequenceNumber == 0)
        {
            throw new ArgumentException("Account sequence numbers must be at least 1.");
        }
        return new AccountSequenceNumber(sequenceNumber);
    }

    /// <summary>
    /// Returns a new nonce whose value is increased by 1 relative to the current nonce.
    /// </summary>
    /// <exception cref="OverflowException">The value of the incremented nonce does not fit in a 64-bit unsigned integer.</exception>
    public AccountSequenceNumber GetIncrementedNonce()
    {
        if (Value == UInt64.MaxValue)
        {
            throw new OverflowException(
                "Value of the incremented nonce does not fit in a 64-bit unsigned integer."
            );
        }
        return new AccountSequenceNumber(Value + 1);
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

    public static implicit operator AccountSequenceNumber(UInt64 value)
    {
        return new AccountSequenceNumber(value);
    }

    public static implicit operator UInt64(AccountSequenceNumber byteIndex)
    {
        return byteIndex.Value;
    }

    public bool Equals(AccountSequenceNumber other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is AccountSequenceNumber other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
