namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a update queues version 0.
/// </summary>
public record UpdateQueuesV0 : UpdateQueues
{
    /// <summary>
    /// Gets or initiates the baker stake threshold.
    /// </summary>
    public UpdateQueue BakerStakeThreshold { get; init; }
}
