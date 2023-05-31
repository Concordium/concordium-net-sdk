namespace Concordium.Sdk.Types.New;

public record GasRewards(
    decimal Baker,
    decimal FinalizationProof,
    decimal AccountCreation,
    decimal ChainUpdate);