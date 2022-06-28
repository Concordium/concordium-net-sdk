using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: look at js sdk think can we add default checks for some type properties or enum values
/// <summary>
/// Represents the information about a transaction status as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetTransactionStatusAsync"/>.
/// </summary>
public record TransactionStatus
{
    /// <summary>
    /// Gets or initiates the transaction status type.
    /// </summary>
    public TransactionStatusType Status { get; init; }

    /// <summary>
    /// Gets or initiates the outcomes dictionary.
    /// </summary>
    public Dictionary<string, TransactionSummary>? Outcomes { get; init; }
}
