namespace Concordium.Sdk.Types;

/// <summary>Transaction time specified as seconds since unix epoch.</summary>
/// <param name="SecondsSinceUnixEpoch">Seconds since the unix epoch.</param>
public sealed record TransactionTime(ulong SecondsSinceUnixEpoch)
{
    internal static TransactionTime From(Grpc.V2.TransactionTime transactionTime) =>
        new(transactionTime.Value);

    /// <summary>Convert the TransactionTime to a DateTimeOffset.</summary>
    public DateTimeOffset ToDateTimeOffset()
    {
        if (this.SecondsSinceUnixEpoch > long.MaxValue)
        {
            throw new ArgumentOutOfRangeException(
                $"The timestamp has a value of {this.SecondsSinceUnixEpoch} which exceeds the maximum value of supported by DateTimeOffset."
            );
        }
        else
        {
            return DateTimeOffset.FromUnixTimeSeconds((long)this.SecondsSinceUnixEpoch);
        }
    }
}
