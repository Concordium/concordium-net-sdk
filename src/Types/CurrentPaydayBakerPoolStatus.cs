using Concordium.Grpc.V2;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Status of the pool in the current reward period.
/// </summary>
/// <param name="BlocksBaked">
/// The number of blocks baked in the current reward period.
/// </param>
/// <param name="FinalizationLive">
/// Whether the baker has contributed a finalization proof in the current
/// reward period.
/// </param>
/// <param name="TransactionFeesEarned">
/// The transaction fees accruing to the pool in the current reward period.
/// </param>
/// <param name="EffectiveStake">
/// The effective stake of the baker in the current reward period.
/// </param>
/// <param name="LotteryPower">
/// The lottery power of the baker in the current reward period.
/// </param>
/// <param name="BakerEquityCapital">
/// The effective equity capital of the baker for the current reward period.
/// </param>
/// <param name="DelegatedCapital">
/// The effective delegated capital to the pool for the current reward
/// period.
/// </param>
public sealed record CurrentPaydayBakerPoolStatus(
    ulong BlocksBaked,
    bool FinalizationLive,
    CcdAmount TransactionFeesEarned,
    CcdAmount EffectiveStake,
    decimal LotteryPower,
    CcdAmount BakerEquityCapital,
    CcdAmount DelegatedCapital)
{
    internal static CurrentPaydayBakerPoolStatus? From(PoolCurrentPaydayInfo? currentPaydayInfo)
    {
        if (currentPaydayInfo is null)
        {
            return null;
        }

        return new CurrentPaydayBakerPoolStatus(
            currentPaydayInfo.BlocksBaked,
            currentPaydayInfo.FinalizationLive,
            currentPaydayInfo.TransactionFeesEarned.ToCcd(),
            currentPaydayInfo.EffectiveStake.ToCcd(),
            (decimal)currentPaydayInfo.LotteryPower,
            currentPaydayInfo.BakerEquityCapital.ToCcd(),
            currentPaydayInfo.DelegatedCapital.ToCcd());
    }
}
