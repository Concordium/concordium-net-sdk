namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a transfer with memo event result.
/// </summary>
public record TransferWithMemoEventResult : EventResult
{
    /// <summary>
    /// Gets or initiates the outcome.
    /// </summary>
    public string Outcome { get; init; }

    /// <summary>
    /// Gets or initiates the events.
    /// </summary>
    public (TransferredEvent, MemoEvent) Events { get; init; }
}
