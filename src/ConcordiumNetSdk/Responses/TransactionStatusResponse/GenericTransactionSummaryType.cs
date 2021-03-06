namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a generic transaction summary type.
/// </summary>
public record GenericTransactionSummaryType : BaseTransactionSummaryType
{
    /// <summary>
    /// Gets or initiates the contents.
    /// </summary>
    public string Contents { get; init; }
}
