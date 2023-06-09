namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Reward Overview version 0.
/// Only exists for protocol versions 1, 2, and 3.
/// </summary>
public record RewardOverviewV0(
    Mapped.ProtocolVersion ProtocolVersion,
    CcdAmount TotalAmount,
    CcdAmount TotalEncryptedAmount,
    CcdAmount BakingRewardAccount,
    CcdAmount FinalizationRewardAccount,
    CcdAmount GasAccount) :
    RewardOverviewBase(ProtocolVersion, TotalAmount, TotalEncryptedAmount, BakingRewardAccount, FinalizationRewardAccount, GasAccount);
