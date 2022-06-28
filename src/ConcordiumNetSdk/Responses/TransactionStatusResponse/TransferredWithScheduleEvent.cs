namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a transferred with schedule event.
/// </summary>
public record TransferredWithScheduleEvent : Event
{
    /// <summary>
    /// Gets or initiates the tag.
    /// </summary>
    public string Tag { get; init; }

    /// <summary>
    /// Gets or initiates the to account address.
    /// </summary>
    public AccountAddressInfo To { get; init; }

    /// <summary>
    /// Gets or initiates the from account address.
    /// </summary>
    public AccountAddressInfo From { get; init; }

    /// <summary>
    /// Gets or initiates the release schedules amount.
    /// </summary>
    public List<ReleaseSchedule> Amount { get; init; }
}
