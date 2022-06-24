namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a updates version 1.
/// </summary>
public record UpdatesV1 : Updates
{
    /// <summary>
    /// Gets or initiates the chain parameters.
    /// </summary>
    public ChainParametersV1 ChainParameters { get; init; }

    /// <summary>
    /// Gets or initiates the update queues.
    /// </summary>
    public UpdateQueuesV1 UpdateQueues { get; init; }

    /// <summary>
    /// Gets or initiates the keys.
    /// </summary>
    public KeysV1 Keys { get; init; }
}
