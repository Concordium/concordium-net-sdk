namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a successful event result.
/// </summary>
public record SuccessfulEventResult : EventResult
{
    /// <summary>
    /// Gets or initiates the outcome.
    /// </summary>
    public string Outcome { get; init; }

    /// <summary>
    /// Gets or initiates the events.
    /// </summary>
    public List<Event> Events { get; init; }
}
