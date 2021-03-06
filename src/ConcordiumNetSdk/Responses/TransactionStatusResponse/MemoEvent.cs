namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a memo event.
/// </summary>
public record MemoEvent : Event
{
    /// <summary>
    /// Gets or initiates the tag.
    /// </summary>
    public string Tag { get; init; }

    /// <summary>
    /// Gets or initiates the memo.
    /// </summary>
    public string Memo { get; init; }
}
