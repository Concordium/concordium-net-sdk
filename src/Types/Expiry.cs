using System.Globalization;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an expiration time for a transaction.
///
/// A transaction can not be included in a block if its expiration time is before the block (slot) time.
/// </summary>
public readonly struct Expiry : IEquatable<Expiry>
{
    public const int BytesLength = sizeof(ulong);

    /// <summary>
    /// Time at which the transaction expires.
    /// </summary>
    private readonly DateTimeOffset _timestamp;

    /// <summary>
    /// Initializes a new instance of the <see cref="Expiry"/> class.
    /// </summary>
    /// <param name="timestamp">Time at which the transaction expires.</param>
    private Expiry(DateTimeOffset timestamp) => this._timestamp = timestamp;

    /// <summary>
    /// Creates a new expiration time that is <paramref name="minutes"/> ahead of the current system time.
    /// </summary>
    /// <param name="minutes">An expiration time specified by the number of minutes from the current system time.</param>
    public static Expiry AtMinutesFromNow(ulong minutes) => From(Now().AddMinutes(minutes));

    /// <summary>
    /// Creates a new expiration time that is <paramref name="seconds"/> from the current system time.
    /// </summary>
    /// <param name="seconds">An expiration time specified by the number of seconds from the current system time.</param>
    public static Expiry AtSecondsFromNow(ulong seconds) => From(Now().AddSeconds(seconds));

    /// <summary>
    /// Creates an instance from a expiration time represented by the elapsed number of seconds since the UNIX epoch.
    /// </summary>
    /// <param name="secondsSinceEpoch">Expiration time represented by the elapsed seconds since the UNIX epoch.</param>
    public static Expiry From(ulong secondsSinceEpoch)
    {
        // We check due to the type of FromUnixTimeSeconds.
        if (secondsSinceEpoch > long.MaxValue)
        {
            throw new ArgumentOutOfRangeException(
                $"The supplied UInt64 timestamp has a value of {secondsSinceEpoch} exceeding the maximum value of Int64."
            );
        }
        return new Expiry(DateTimeOffset.FromUnixTimeSeconds((long)secondsSinceEpoch));
    }

    /// <summary>
    /// Creates an instance whose expiration time is the current system time.
    /// </summary>
    public static DateTimeOffset Now()
    {
        var now = DateTimeOffset.UtcNow;
        // UInt64 is used to represent the number of seconds since the UNIX epoch in API.
        if (now.ToUnixTimeSeconds() < 0)
        {
            throw new ArgumentOutOfRangeException(
                "System time as number of seconds since UNIX epoch must be larger than 0."
            );
        }
        return now;
    }

    /// <summary>
    /// Creates an instance from a <see cref="DateTime"/>.
    /// </summary>
    public static Expiry From(DateTime date) => new(new DateTimeOffset(date));

    /// <summary>
    /// Creates an instance from a <see cref="DateTimeOffset"/>.
    /// </summary>
    public static Expiry From(DateTimeOffset timestamp) => new(timestamp);

    /// <summary>
    /// Get the expiration time represented by the elapsed number of seconds since
    /// the UNIX epoch.
    /// </summary>
    public ulong GetValue() => (ulong)this._timestamp.ToUnixTimeSeconds();

    /// <summary>
    /// Get the expiration time represented by the elapsed number of seconds since
    /// the UNIX epoch written as a 64-bit integer in big-endian format.
    /// </summary>
    public byte[] GetBytes() => Serialization.GetBytes(this.GetValue());

    /// <summary>
    /// Get a string representation of the date and time in the calendar used by the current culture.
    /// </summary>
    public override string ToString() => this._timestamp.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the expiration time to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="Client.RawClient"/>.
    /// </summary>
    public Grpc.V2.TransactionTime ToProto() =>
        new() { Value = (ulong)this._timestamp.ToUnixTimeSeconds() };

    public bool Equals(Expiry other) => this._timestamp.EqualsExact(other._timestamp);

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        var other = (Expiry)obj;
        return this.Equals(other);
    }

    public override int GetHashCode() => this._timestamp.GetHashCode();

    public static bool operator ==(Expiry? left, Expiry? right) => Equals(left, right);

    public static bool operator !=(Expiry? left, Expiry? right) => !Equals(left, right);

    public static bool operator <(Expiry left, Expiry right) => left._timestamp < right._timestamp;

    public static bool operator >(Expiry left, Expiry right) => left._timestamp > right._timestamp;

    public static bool operator <=(Expiry left, Expiry right) =>
        left._timestamp <= right._timestamp;

    public static bool operator >=(Expiry left, Expiry right) =>
        left._timestamp >= right._timestamp;
}
