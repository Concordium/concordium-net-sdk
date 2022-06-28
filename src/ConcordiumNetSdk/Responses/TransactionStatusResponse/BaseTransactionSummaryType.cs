namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

/// <summary>
/// Represents the information about a base transaction summary type.
/// </summary>
public abstract record BaseTransactionSummaryType
{
    /// <summary>
    /// Gets or initiates the type.
    /// </summary>
    public string Type { get; init; }
}
