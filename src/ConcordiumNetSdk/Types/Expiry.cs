using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types
{
    /// <summary>
    /// Expiration time for a transaction.
    /// </summary>
    public class Expiry
    {
        public const int BytesLength = sizeof(UInt64);

        /// <summary>
        /// Time at which the transaction expires.
        /// </summary>
        private readonly DateTimeOffset _timestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="Expiry"/> class.
        /// </summary>
        /// <param name="timestamp">Time at which the transaction expires.</param>
        private Expiry(DateTimeOffset timestamp)
        {
            _timestamp = timestamp;
        }

        /// <summary>
        /// Returns a new expiration time that is <c>minutes</c> ahead of the receiver object.
        /// </summary>
        /// <param name="minutes">Number of minutes later the resulting expiration times should be, relative to <c>this</c>.</param>
        public Expiry AddMinutes(UInt64 minutes)
        {
            return Expiry.From(_timestamp.AddMinutes(minutes));
        }

        /// <summary>
        /// Returns a new expiration time that is <c>seconds</c> ahead of the receiver object.
        /// </summary>
        /// <param name="minutes">Number of seconds later the resulting expiration times should be, relative to <c>this</c>.</param>
        public Expiry AddSeconds(UInt64 seconds)
        {
            return Expiry.From(_timestamp.AddSeconds(seconds));
        }

        /// <summary>
        /// Creates an instance from a expiration time represented by the elapsed number of seconds since the UNIX epoch.
        /// </summary>
        /// <param name="secondsSinceEpoch">Expiration time represented by the elapsed seconds since the UNIX epoch.</param>
        public static Expiry From(UInt64 secondsSinceEpoch)
        {
            // We check due to the type of FromUnixTimeSeconds.
            if (secondsSinceEpoch > Int64.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    "UInt64 timestamp value exceeds maximum value of Int64."
                );
            }
            return new Expiry(DateTimeOffset.FromUnixTimeSeconds((Int64)secondsSinceEpoch));
        }

        /// <summary>
        /// Creates an instance whose expiration time is the current system time.
        /// </summary>
        public static Expiry Now()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // UInt64 is used to represent the number of seconds since the UNIX epoch in API.
            if (now < 0)
            {
                throw new ArgumentOutOfRangeException("Timestamp must be larger than 0.");
            }
            return Expiry.From((UInt64)now);
        }

        /// <summary>
        /// Creates an instance from a <see cref="DateTime"/>.
        /// </summary>
        public static Expiry From(DateTime date)
        {
            return new Expiry(new DateTimeOffset(date));
        }

        /// <summary>
        /// Creates an instance from a <see cref="DateTimeOffset"/>.
        /// </summary>
        public static Expiry From(DateTimeOffset timestamp)
        {
            return new Expiry(timestamp);
        }

        /// <summary>
        /// Get the expiration time represented by the elapsed number of seconds since
        /// the UNIX epoch.
        /// </summary>
        public UInt64 GetValue()
        {
            return (UInt64)_timestamp.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Get the expiration time represented by the elapsed number of seconds since
        /// the UNIX epoch written as a 64-bit integer in big-endian format.
        /// </summary>
        public byte[] GetBytes()
        {
            return Serialization.GetBytes(this.GetValue());
        }

        /// <summary>
        /// Get a string representation of the date and time in the calendar used by the current culture.
        /// </summary>
        public override string ToString()
        {
            return _timestamp.ToString();
        }

        /// <summary>
        /// Converts the expiration time to its corresponding protocol buffer message instance.
        /// </summary>
        public Concordium.V2.TransactionTime ToProto()
        {
            return new Concordium.V2.TransactionTime()
            {
                Value = (UInt64)_timestamp.ToUnixTimeSeconds()
            };
        }
    }
}
