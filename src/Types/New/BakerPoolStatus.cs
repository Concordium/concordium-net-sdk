namespace Concordium.Sdk.Types.New;

public record BakerPoolStatus(ulong BakerId,
        AccountAddress BakerAddress,
        CcdAmount BakerEquityCapital,
        CcdAmount DelegatedCapital,
        CcdAmount DelegatedCapitalCap,
        BakerPoolInfo PoolInfo,
        CurrentPaydayBakerPoolStatus? CurrentPaydayStatus,
        CcdAmount AllPoolTotalCapital)
{
        internal static BakerPoolStatus From(Concordium.Grpc.V2.PoolInfoResponse poolInfoResponse)
        {
            return new BakerPoolStatus(
                poolInfoResponse.Baker.Value,
                AccountAddress.From(poolInfoResponse.Address),
                CcdAmount.From(poolInfoResponse.EquityCapital),
                CcdAmount.From(poolInfoResponse.DelegatedCapital),
                CcdAmount.From(poolInfoResponse.DelegatedCapitalCap),
                BakerPoolInfo.From(poolInfoResponse.PoolInfo),
                CurrentPaydayBakerPoolStatus.From(poolInfoResponse.CurrentPaydayInfo),
                CcdAmount.From(poolInfoResponse.AllPoolTotalCapital));
        }
}


