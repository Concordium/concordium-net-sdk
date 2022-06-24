namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a update queue.
/// </summary>
public record UpdateQueue
{
    /// <summary>
    /// Gets or initiates the next sequence number.
    /// </summary>
    public ulong NextSequenceNumber { get; init; }

    /// <summary>
    /// Gets or initiates the queue.
    /// </summary>
    public List<UpdateQueueQueue> Queue { get; init; }
}
