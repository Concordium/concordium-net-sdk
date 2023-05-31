namespace Concordium.Sdk.Types.New;

public record UpdateQueue<T>(
    ulong NextSequenceNumber,
    ScheduledUpdate<T>[] Queue);
