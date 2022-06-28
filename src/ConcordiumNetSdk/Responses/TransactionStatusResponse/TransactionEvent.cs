namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a transaction event.
/// </summary>
public record TransactionEvent : Event
{
    /// <summary>
    /// Gets or initiates the tag.
    /// </summary>
    public string Tag { get; init; }
}
