namespace Concordium.Sdk.Types.New;

public record UpdateKeysCollectionV1(
    HigherLevelAccessStructureRootKeys RootKeys,
    HigherLevelAccessStructureLevel1Keys Level1Keys,
    AuthorizationsV1 Level2Keys);