using Concordium.Sdk.Client;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an account sequence number.
///
/// The account sequence number is a 64-bit unsigned integer specific to an account
/// and used when submitting account transactions for that account to the node. The account
/// sequence number is maintained as on-chain state and is incremented with each finalized
/// transaction. The next sequence number to be used in a transaction can be queried
/// with <see cref="ConcordiumClient.GetNextAccountSequenceNumber"/>.
/// </summary>
public readonly struct AccountSequenceNumber : IEquatable<AccountSequenceNumber>
{
    public const uint BytesLength = sizeof(ulong);

    /// <summary>
    /// The value of the account sequence number.
    /// </summary>
    public readonly ulong Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountSequenceNumber"/> class.
    /// </summary>
    /// <param name="sequenceNumber">The sequence number.</param>
    private AccountSequenceNumber(ulong sequenceNumber) => this.Value = sequenceNumber;

    /// <summary>
    /// Creates a new instance of the <see cref="AccountSequenceNumber"/> class from an integer.
    /// </summary>
    /// <param name="sequenceNumber">The account sequence number represented as an integer.</param>
    /// <exception cref="ArgumentException">The supplied account sequence number is 0.</exception>
    public static AccountSequenceNumber From(ulong sequenceNumber)
    {
        if (sequenceNumber == 0)
        {
            throw new ArgumentException("Account sequence numbers must be at least 1.");
        }
        return new AccountSequenceNumber(sequenceNumber);
    }

    /// <summary>
    /// Returns a new sequence number whose value is increased by 1 relative to the current one.
    /// </summary>
    /// <exception cref="OverflowException">The value of the incremented sequence number does not fit in a 64-bit unsigned integer.</exception>
    public AccountSequenceNumber GetIncrementedSequenceNumber()
    {
        if (this.Value == ulong.MaxValue)
        {
            throw new OverflowException(
                "Value of the incremented sequence number does not fit in a 64-bit unsigned integer."
            );
        }
        return new AccountSequenceNumber(this.Value + 1);
    }

    /// <summary>
    /// Copies the account sequence represented in big-endian format to a byte array.
    /// </summary>
    public byte[] ToBytes() => Serialization.ToBytes(this.Value);

    /// <summary>
    /// Converts the account sequence number to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="RawClient"/>.
    /// </summary>
    public Grpc.V2.SequenceNumber ToProto() => new() { Value = Value };

    public static implicit operator AccountSequenceNumber(ulong value) => new(value);

    public static implicit operator ulong(AccountSequenceNumber byteIndex) => byteIndex.Value;

    public bool Equals(AccountSequenceNumber other) => this.Value == other.Value;

    public override bool Equals(object? obj) => obj is AccountSequenceNumber other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public static bool operator ==(AccountSequenceNumber left, AccountSequenceNumber right) => left.Equals(right);

    public static bool operator !=(AccountSequenceNumber left, AccountSequenceNumber right) => !(left == right);
}
