namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a transfer with memo summary type.
/// </summary>
public record TransferWithMemoSummaryType : BaseTransactionSummaryType
{
    /// <summary>
    /// Gets or initiates the contents.
    /// </summary>
    public string Contents { get; init; }
}
