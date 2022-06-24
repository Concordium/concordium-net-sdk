namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a finalization data.
/// </summary>
public record FinalizationData
{
    /// <summary>
    /// Gets or initiates the finalization index.
    /// </summary>
    public ulong FinalizationIndex { get; init; }

    /// <summary>
    /// Gets or initiates the finalization delay.
    /// </summary>
    public ulong FinalizationDelay { get; init; }

    /// <summary>
    /// Gets or initiates the finalization block pointer.
    /// </summary>
    public string FinalizationBlockPointer { get; init; }

    /// <summary>
    /// Gets or initiates the finalizers.
    /// </summary>
    public List<PartyInfo> Finalizers { get; init; }
}
