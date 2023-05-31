namespace Concordium.Sdk.Types.New;

public record HigherLevelAccessStructureRootKeys(
    UpdatePublicKey[] Keys,
    ushort Threshold);