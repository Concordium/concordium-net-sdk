namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a block summary version 0.
/// </summary>
public record BlockSummaryV0 : BlockSummary
{
    /// <summary>
    /// Gets or initiates the updates.
    /// </summary>
    public UpdatesV0 Updates { get; init; }
}
