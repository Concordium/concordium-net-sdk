namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a rejected reason.
/// </summary>
public record RejectReason
{
    /// <summary>
    /// Gets or initiates the reject reason tag.
    /// </summary>
    public RejectReasonTag Tag { get; init; }

    //todo: think of better way to deserialize this data or leave some json object
    /// <summary>
    /// Gets or initiates the contents.
    /// </summary>
    public object Contents { get; init; }
}
