using ConcordiumNetSdk.Responses.TransactionStatusResponse;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.TransactionStatusInBlockResponse;

/// <summary>
/// Represents the information about a transaction status in block as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetTransactionStatusInBlockAsync"/>.
/// </summary>
public record TransactionStatusInBlock
{
    /// <summary>
    /// Gets or initiates the transaction status type.
    /// </summary>
    public TransactionStatusType Status { get; init; }

    /// <summary>
    /// Gets or initiates the result.
    /// </summary>
    public TransactionSummary? Result { get; init; }
}
