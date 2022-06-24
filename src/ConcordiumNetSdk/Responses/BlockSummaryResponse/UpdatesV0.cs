namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a updates version 0.
/// </summary>
public record UpdatesV0 : Updates
{
    /// <summary>
    /// Gets or initiates the chain parameters.
    /// </summary>
    public ChainParametersV0 ChainParameters { get; init; }

    /// <summary>
    /// Gets or initiates the update queues.
    /// </summary>
    public UpdateQueuesV0 UpdateQueues { get; init; }

    /// <summary>
    /// Gets or initiates the keys.
    /// </summary>
    public KeysV0 Keys { get; init; }
}
