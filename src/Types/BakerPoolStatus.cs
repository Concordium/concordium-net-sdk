namespace Concordium.Sdk.Types;

/// <summary>
/// The state of the baker currently registered on the account.
/// Current here means "present". This is the information that is being updated
/// by transactions (and rewards). This is in contrast to "epoch baker" which is
/// the state of the baker that is currently eligible for baking.
/// </summary>
/// <remarks>
/// From protocol version 7, pool removal has immediate effect, however, the
/// pool may still be present for the current (and possibly next) reward period.
/// </remarks>
/// <param name="BakerId">The 'BakerId' of the pool owner.</param>
/// <param name="BakerAddress">The account address of the pool owner.</param>
/// <param name="BakerEquityCapital">The equity capital provided by the pool owner. Absent if the pool is removed.</param>
/// <param name="DelegatedCapital">The capital delegated to the pool by other accounts. Absent if the pool is removed.</param>
/// <param name="DelegatedCapitalCap">
/// The maximum amount that may be delegated to the pool, accounting for
/// leverage and stake limits.
/// Absent if the pool is removed.
/// </param>
/// <param name="PoolInfo">
/// The pool info associated with the pool: open status, metadata URL
/// and commission rates.
/// Absent if the pool is removed.
/// </param>
/// <param name="CurrentPaydayStatus">
/// Status of the pool in the current reward period. This will be null
/// if the pool is not a baker in the payday (e.g., because they just
/// registered and a new payday has not started yet).
/// </param>
/// <param name="BakerStakePendingChange">Any pending change to the baker's stake.</param>
/// <param name="AllPoolTotalCapital">Total capital staked across all pools.</param>
/// <param name="IsSuspended">
/// A flag indicating whether the pool owner is suspended.
/// Also `False` if the protocol version does not support validator suspension or the pool is removed.
/// </param>
public sealed record BakerPoolStatus(
        BakerId BakerId,
        AccountAddress BakerAddress,
        CcdAmount? BakerEquityCapital,
        CcdAmount? DelegatedCapital,
        CcdAmount? DelegatedCapitalCap,
        BakerPoolInfo? PoolInfo,
        CurrentPaydayBakerPoolStatus? CurrentPaydayStatus,
        CcdAmount AllPoolTotalCapital,
        BakerPoolPendingChange? BakerStakePendingChange,
        bool IsSuspended
)
{
    internal static BakerPoolStatus From(Grpc.V2.PoolInfoResponse poolInfoResponse) =>
        new(
            BakerId.From(poolInfoResponse.Baker),
            AccountAddress.From(poolInfoResponse.Address),
            poolInfoResponse.EquityCapital != null ? CcdAmount.From(poolInfoResponse.EquityCapital) : null,
            poolInfoResponse.DelegatedCapital != null ? CcdAmount.From(poolInfoResponse.DelegatedCapital) : null,
            poolInfoResponse.DelegatedCapitalCap != null ? CcdAmount.From(poolInfoResponse.DelegatedCapitalCap) : null,
            poolInfoResponse.PoolInfo != null ? BakerPoolInfo.From(poolInfoResponse.PoolInfo) : null,
            CurrentPaydayBakerPoolStatus.From(poolInfoResponse.CurrentPaydayInfo),
            CcdAmount.From(poolInfoResponse.AllPoolTotalCapital),
            BakerPoolPendingChange.From(poolInfoResponse.EquityPendingChange),
            poolInfoResponse.IsSuspended
        );
}
