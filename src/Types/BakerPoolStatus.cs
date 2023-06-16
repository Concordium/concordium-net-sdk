namespace Concordium.Sdk.Types;

/// <summary>
/// The state of the baker currently registered on the account.
/// Current here means "present". This is the information that is being updated
/// by transactions (and rewards). This is in contrast to "epoch baker" which is
/// the state of the baker that is currently eligible for baking.
/// </summary>
/// <param name="BakerId">The 'BakerId' of the pool owner.</param>
/// <param name="BakerAddress">The account address of the pool owner.</param>
/// <param name="BakerEquityCapital">The equity capital provided by the pool owner.</param>
/// <param name="DelegatedCapital">The capital delegated to the pool by other accounts.</param>
/// <param name="DelegatedCapitalCap">
/// The maximum amount that may be delegated to the pool, accounting for
/// leverage and stake limits.
/// </param>
/// <param name="PoolInfo">
/// The pool info associated with the pool: open status, metadata URL
/// and commission rates.
/// </param>
/// <param name="CurrentPaydayStatus">
/// Status of the pool in the current reward period. This will be null
/// if the pool is not a baker in the payday (e.g., because they just
/// registered and a new payday has not started yet).
/// </param>
/// <param name="AllPoolTotalCapital">Total capital staked across all pools.</param>
public record BakerPoolStatus(ulong BakerId,
        AccountAddress BakerAddress,
        CcdAmount BakerEquityCapital,
        CcdAmount DelegatedCapital,
        CcdAmount DelegatedCapitalCap,
        BakerPoolInfo PoolInfo,
        CurrentPaydayBakerPoolStatus? CurrentPaydayStatus,
        CcdAmount AllPoolTotalCapital)
{
        internal static BakerPoolStatus From(Concordium.Grpc.V2.PoolInfoResponse poolInfoResponse) =>
            new(
                poolInfoResponse.Baker.Value,
                AccountAddress.From(poolInfoResponse.Address),
                CcdAmount.From(poolInfoResponse.EquityCapital),
                CcdAmount.From(poolInfoResponse.DelegatedCapital),
                CcdAmount.From(poolInfoResponse.DelegatedCapitalCap),
                BakerPoolInfo.From(poolInfoResponse.PoolInfo),
                CurrentPaydayBakerPoolStatus.From(poolInfoResponse.CurrentPaydayInfo),
                CcdAmount.From(poolInfoResponse.AllPoolTotalCapital));
}


