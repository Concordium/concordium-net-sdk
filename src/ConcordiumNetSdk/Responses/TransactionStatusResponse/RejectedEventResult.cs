namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a rejected event result.
/// </summary>
public record RejectedEventResult : EventResult
{
    /// <summary>
    /// Gets or initiates the outcome.
    /// </summary>
    public string Outcome { get; init; }

    /// <summary>
    /// Gets or initiates the reject reason.
    /// </summary>
    public RejectReason RejectReason { get; init; }
}
