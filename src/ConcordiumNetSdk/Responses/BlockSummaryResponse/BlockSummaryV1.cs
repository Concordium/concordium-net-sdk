namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a block summary version 1.
/// </summary>
public record BlockSummaryV1 : BlockSummary
{
    /// <summary>
    /// Gets or initiates the updates.
    /// </summary>
    public UpdatesV1 Updates { get; init; }
    
    /// <summary>
    /// Gets or initiates the protocol version.
    /// </summary>
    public ulong ProtocolVersion { get; init; }
}
