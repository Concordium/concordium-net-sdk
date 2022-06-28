namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a release schedule.
/// </summary>
public record ReleaseSchedule
{
    /// <summary>
    /// Gets or initiates the timestamp.
    /// </summary>
    public DateTime Timestamp { get; init; }

    //todo: use long or ulong for it, investigate serialization and deserialization, make custom converter
    /// <summary>
    /// Gets or initiates the amount.
    /// </summary>
    public string Amount { get; init; }
}
