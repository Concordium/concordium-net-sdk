namespace Concordium.Sdk.Types.New;

public record RewardParametersV0(
    MintDistributionV0 MintDistribution,
    TransactionFeeDistribution TransactionFeeDistribution,
    GasRewards GASRewards);