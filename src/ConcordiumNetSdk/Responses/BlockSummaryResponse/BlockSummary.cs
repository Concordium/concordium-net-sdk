using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.TransactionStatusResponse;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the base class for block summary as response data of <see cref="ConcordiumNodeClient"/>.<see cref="ConcordiumNodeClient.GetBlockSummaryAsync"/>.
/// </summary>
public abstract record BlockSummary
{
    /// <summary>
    /// Gets or initiates the protocol version.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ulong? ProtocolVersion { get; init; }

    /// <summary>
    /// Gets or initiates the finalization data.
    /// </summary>
    public FinalizationData FinalizationData { get; init; }

    /// <summary>
    /// Gets or initiates the transaction summaries.
    /// </summary>
    public List<TransactionSummary> TransactionSummaries { get; init; }
}
