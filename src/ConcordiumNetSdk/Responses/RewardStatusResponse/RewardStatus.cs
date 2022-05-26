using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.RewardStatusResponse;

/// <summary>
/// Represents the information about a current balance of special accounts as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetRewardStatusAsync"/>.
/// See <a href="https://github.com/Concordium/concordium-node/edit/main/docs/grpc.md#getrewardstatus-blockhash---rewardstatus">here</a>.
/// </summary>
public record RewardStatus
{
    /// <summary>
    /// Gets or initiates the total amount of currency in existence at the end of this block.
    /// </summary>
    public CcdAmount TotalAmount { get; init; }

    /// <summary>
    /// Gets or initiates the total amount of encrypted amounts in existence at the end of the block.
    /// </summary>
    public CcdAmount TotalEncryptedAmount { get; init; }

    /// <summary>
    /// Gets or initiates the balance of the GAS account.
    /// </summary>
    public CcdAmount GasAccount { get; init; }

    /// <summary>
    /// Gets or initiates the balance of the baking reward account.
    /// </summary>
    public CcdAmount BakingRewardAccount { get; init; }

    /// <summary>
    /// Gets or initiates the balance of the finalization reward account.
    /// </summary>
    public CcdAmount FinalizationRewardAccount { get; init; }
}
