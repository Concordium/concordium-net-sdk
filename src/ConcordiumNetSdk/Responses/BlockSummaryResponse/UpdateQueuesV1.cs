namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a updates queues version 1.
/// </summary>
public record UpdateQueuesV1 : UpdateQueues
{
    /// <summary>
    /// Gets or initiates the cooldown parameters.
    /// </summary>
    public UpdateQueue CooldownParameters { get; init; }

    /// <summary>
    /// Gets or initiates the time parameters.
    /// </summary>
    public UpdateQueue TimeParameters { get; init; }

    /// <summary>
    /// Gets or initiates the pool parameters.
    /// </summary>
    public UpdateQueue PoolParameters { get; init; }
}
