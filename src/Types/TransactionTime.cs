namespace Concordium.Sdk.Types;

/// <summary>Transaction time specified as seconds since unix epoch.</summary>
/// <param name="SecondsSinceUnixEpoch">Seconds since the unix epoch.</param>
public sealed record TransactionTime(ulong SecondsSinceUnixEpoch)
{
    internal static TransactionTime From(Grpc.V2.TransactionTime transactionTime) =>
        new TransactionTime(transactionTime.Value);

    /// <summary>Convert the TransactionTime to a DateTimeOffset.</summary>
    public DateTimeOffset toDateTimeOffset() => 
        DateTimeOffset.FromUnixTimeSeconds((long) this.SecondsSinceUnixEpoch);
}
