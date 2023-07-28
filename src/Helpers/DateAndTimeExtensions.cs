using Concordium.Grpc.V2;

namespace Concordium.Sdk.Helpers;

internal static class DateAndTimeExtensions
{
    internal static DateTimeOffset ToDateTimeOffset(this Timestamp timestamp) => DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp.Value);

    internal static DateTimeOffset ToDateTimeOffset(this TransactionTime seconds) => DateTimeOffset.FromUnixTimeSeconds((long)seconds.Value);

    /// <summary>
    /// Durations from mapped from GRPC are given in milliseconds.
    /// </summary>
    /// <returns></returns>
    internal static TimeSpan ToTimeSpan(this Duration duration) => TimeSpan.FromMilliseconds(duration.Value);
}
