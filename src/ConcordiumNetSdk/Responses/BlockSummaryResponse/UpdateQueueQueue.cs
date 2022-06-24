namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a update queue queue.
/// </summary>
public record UpdateQueueQueue
{
    /// <summary>
    /// Gets or initiates the effective time.
    /// </summary>
    public DateTime EffectiveTime { get; init; }

    /// <summary>
    /// Gets or initiates the update.
    /// </summary>
    public object Update { get; init; }
}
