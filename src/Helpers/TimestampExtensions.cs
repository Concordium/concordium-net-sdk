using Concordium.Grpc.V2;

namespace Concordium.Sdk.Helpers;

internal static class TimestampExtensions
{
    internal static DateTimeOffset ToDateTimeOffset(this Timestamp timestamp) => DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp.Value);

    internal static DateTimeOffset ToDateTimeOffset(this TransactionTime seconds) => DateTimeOffset.FromUnixTimeSeconds((long)seconds.Value);
}
