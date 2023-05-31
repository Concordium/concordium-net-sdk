namespace Concordium.Sdk.Types.New;

public record RewardParametersV1(
    MintDistributionV1 MintDistribution,
    TransactionFeeDistribution TransactionFeeDistribution,
    GasRewards GASRewards);