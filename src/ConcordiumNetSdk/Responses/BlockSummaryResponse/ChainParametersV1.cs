using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.BlockSummaryResponse;

/// <summary>
/// Represents the information about a chain parameters version 1.
/// </summary>
public record ChainParametersV1 : ChainParameters
{
    /// <summary>
    /// Gets or initiates the reward parameters.
    /// </summary>
    public RewardParametersV1 RewardParameters { get; init; }

    //todo: think to use DurationSeconds class
    /// <summary>
    /// Gets or initiates the pool owner cooldown.
    /// </summary>
    public ulong PoolOwnerCooldown { get; init; }

    //todo: think to use DurationSeconds class
    /// <summary>
    /// Gets or initiates the delegator cooldown.
    /// </summary>
    public ulong DelegatorCooldown { get; init; }

    /// <summary>
    /// Gets or initiates the passive finalization commission.
    /// </summary>
    public int PassiveFinalizationCommission { get; init; }

    /// <summary>
    /// Gets or initiates the passive baking commission.
    /// </summary>
    public int PassiveBakingCommission { get; init; }

    /// <summary>
    /// Gets or initiates the passive transaction commission.
    /// </summary>
    public int PassiveTransactionCommission { get; init; }

    /// <summary>
    /// Gets or initiates the finalization commission range.
    /// </summary>
    public InclusiveRange<int> FinalizationCommissionRange { get; init; }

    /// <summary>
    /// Gets or initiates the baking commission range.
    /// </summary>
    public InclusiveRange<int> BakingCommissionRange { get; init; }

    /// <summary>
    /// Gets or initiates the transaction commission range.
    /// </summary>
    public InclusiveRange<int> TransactionCommissionRange { get; init; }

    /// <summary>
    /// Gets or initiates the minimum equity capital.
    /// </summary>
    public CcdAmount MinimumEquityCapital { get; init; }

    /// <summary>
    /// Gets or initiates the capital bound.
    /// </summary>
    public int CapitalBound { get; init; }

    /// <summary>
    /// Gets or initiates the leverage bound.
    /// </summary>
    public Ratio LeverageBound { get; init; }

    //todo: think about epoch class and check other places
    /// <summary>
    /// Gets or initiates the reward period length.
    /// </summary>
    public ulong RewardPeriodLength { get; init; }

    /// <summary>
    /// Gets or initiates the mint per payday.
    /// </summary>
    public int MintPerPayday { get; init; }
}
