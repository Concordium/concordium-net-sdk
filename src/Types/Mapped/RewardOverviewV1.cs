namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Reward Overview version 1.
/// </summary>
/// <param name="FoundationTransactionRewards">
/// The transaction reward fraction accruing to the foundation (to be
/// paid at next payday).
/// </param>
/// <param name="NextPaydayTime">
/// The time of the next payday.
/// </param>
/// <param name="NextPaydayMintRate">
/// The rate at which CCD will be minted (as a proportion of the total
/// supply) at the next payday
/// </param>
/// <param name="TotalStakedCapital">
/// The total capital put up as stake by bakers and delegators
/// </param>
public record RewardOverviewV1(
    ProtocolVersion ProtocolVersion,
    CcdAmount TotalAmount,
    CcdAmount TotalEncryptedAmount,
    CcdAmount BakingRewardAccount,
    CcdAmount FinalizationRewardAccount,
    CcdAmount GasAccount,
    CcdAmount FoundationTransactionRewards,
    DateTimeOffset NextPaydayTime,
    MintRate NextPaydayMintRate,
    CcdAmount TotalStakedCapital) : RewardOverviewBase(ProtocolVersion, TotalAmount, TotalEncryptedAmount, BakingRewardAccount, FinalizationRewardAccount, GasAccount);
