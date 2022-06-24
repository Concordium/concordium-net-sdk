namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the information about a transfer with memo transaction summary.
/// </summary>
public record TransferWithMemoTransactionSummary : BaseTransactionSummary
{
    /// <summary>
    /// Gets or initiates the transfer with memo summary type.
    /// </summary>
    public TransferWithMemoSummaryType Type { get; init; }
    
    /// <summary>
    /// Gets or initiates the transfer with memo event result.
    /// </summary>
    public TransferWithMemoEventResult Result { get; init; }
}
