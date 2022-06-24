namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a transferred event.
/// </summary>
public record TransferredEvent : Event
{
    /// <summary>
    /// Gets or initiates the tag.
    /// </summary>
    public string Tag { get; init; }

    /// <summary>
    /// Gets or initiates the amount.
    /// </summary>
    public string Amount { get; init; }

    /// <summary>
    /// Gets or initiates the to account address.
    /// </summary>
    public AccountAddressInfo To { get; init; }

    /// <summary>
    /// Gets or initiates the from account address.
    /// </summary>
    public AccountAddressInfo From { get; init; }
}
    
