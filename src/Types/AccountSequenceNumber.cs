using Concordium.Grpc.V2;
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
public readonly record struct AccountSequenceNumber
{
    /// <summary>
    /// The number of bytes used to represent this type when serialized.
    /// </summary>
    public const uint BytesLength = sizeof(ulong);

    /// <summary>
    /// The value of the account sequence number.
    /// </summary>
    public ulong Value { get; init; }

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

    internal static AccountSequenceNumber From(SequenceNumber sequenceNumber) => From(sequenceNumber.Value);

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
    public SequenceNumber ToProto() => new() { Value = this.Value };
}
