using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Information about the state of the CCD distribution at a particular time.
/// Reward data common to both V0 and V1 rewards.
/// </summary>
/// <param name="ProtocolVersion">
/// Protocol version that applies to these rewards. V0 variant
/// </param>
/// <param name="TotalAmount">The total CCD in existence.</param>
/// <param name="TotalEncryptedAmount">The total CCD in encrypted balances.</param>
/// <param name="BakingRewardAccount">The amount in the baking reward account.</param>
/// <param name="FinalizationRewardAccount">The amount in the finalization reward account.</param>
/// <param name="GasAccount">The amount in the GAS account.</param>
public abstract record RewardOverviewBase(
    Mapped.ProtocolVersion ProtocolVersion,
    CcdAmount TotalAmount,
    CcdAmount TotalEncryptedAmount,
    CcdAmount BakingRewardAccount,
    CcdAmount FinalizationRewardAccount,
    CcdAmount GasAccount)
{
    internal static RewardOverviewBase From(TokenomicsInfo tokenomicsInfo) =>
        tokenomicsInfo.TokenomicsCase switch
        {
            TokenomicsInfo.TokenomicsOneofCase.V0 =>
                new RewardOverviewV0(
                    tokenomicsInfo.V0.ProtocolVersion.Into(),
                    tokenomicsInfo.V0.TotalAmount.ToCcd(),
                    tokenomicsInfo.V0.TotalEncryptedAmount.ToCcd(),
                    tokenomicsInfo.V0.BakingRewardAccount.ToCcd(),
                    tokenomicsInfo.V0.FinalizationRewardAccount.ToCcd(),
                    tokenomicsInfo.V0.GasAccount.ToCcd()
                ),
            TokenomicsInfo.TokenomicsOneofCase.V1 =>
                new RewardOverviewV1(
                    tokenomicsInfo.V1.ProtocolVersion.Into(),
                    tokenomicsInfo.V1.TotalAmount.ToCcd(),
                    tokenomicsInfo.V1.TotalEncryptedAmount.ToCcd(),
                    tokenomicsInfo.V1.BakingRewardAccount.ToCcd(),
                    tokenomicsInfo.V1.FinalizationRewardAccount.ToCcd(),
                    tokenomicsInfo.V1.GasAccount.ToCcd(),
                    tokenomicsInfo.V1.FoundationTransactionRewards.ToCcd(),
                    tokenomicsInfo.V1.NextPaydayTime.ToDateTimeOffset(),
                    MintRate.From(tokenomicsInfo.V1.NextPaydayMintRate),
                    tokenomicsInfo.V1.TotalStakedCapital.ToCcd()),
            TokenomicsInfo.TokenomicsOneofCase.None =>
                throw new MissingEnumException<TokenomicsInfo.TokenomicsOneofCase>(tokenomicsInfo.TokenomicsCase),
            _ => throw new MissingEnumException<TokenomicsInfo.TokenomicsOneofCase>(tokenomicsInfo.TokenomicsCase)
        };
}


