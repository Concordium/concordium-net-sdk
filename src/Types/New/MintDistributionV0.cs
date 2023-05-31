namespace Concordium.Sdk.Types.New;

public record MintDistributionV0(
    decimal MintPerSlot,
    decimal BakingReward,
    decimal FinalizationReward);