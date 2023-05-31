using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types.New;

public abstract record RewardStatusBase(
    CcdAmount TotalAmount,
    CcdAmount TotalEncryptedAmount,
    CcdAmount BakingRewardAccount,
    CcdAmount FinalizationRewardAccount,
    CcdAmount GasAccount)
{
    internal static RewardStatusBase From(TokenomicsInfo tokenomicsInfo)
    {
        throw new NotImplementedException();
    }
}
