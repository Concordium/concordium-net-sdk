using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types.New;

public record CurrentPaydayBakerPoolStatus(
    ulong BlocksBaked,
    bool FinalizationLive,
    CcdAmount TransactionFeesEarned,
    CcdAmount EffectiveStake,
    decimal LotteryPower,
    CcdAmount BakerEquityCapital,
    CcdAmount DelegatedCapital)
{
    internal static CurrentPaydayBakerPoolStatus? From(PoolCurrentPaydayInfo currentPaydayInfo)
    {
        throw new NotImplementedException();
    }
}
