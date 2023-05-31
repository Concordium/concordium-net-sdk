namespace Concordium.Sdk.Types.New;

public record ScheduledUpdate<T>(
    UnixTimeSeconds EffectiveTime,
    T Update);
