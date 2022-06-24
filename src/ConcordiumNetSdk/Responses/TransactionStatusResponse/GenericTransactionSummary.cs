namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a generic transaction summary.
/// </summary>
public record GenericTransactionSummary : BaseTransactionSummary
{
    /// <summary>
    /// Gets or initiates the generic transaction summary type.
    /// </summary>
    public GenericTransactionSummaryType Type { get; init; }

    /// <summary>
    /// Gets or initiates the event result.
    /// </summary>
    public EventResult Result { get; init; }
}
