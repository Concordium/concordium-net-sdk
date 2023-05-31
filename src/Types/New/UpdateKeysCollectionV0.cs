namespace Concordium.Sdk.Types.New;

public record UpdateKeysCollectionV0(
    HigherLevelAccessStructureRootKeys RootKeys,
    HigherLevelAccessStructureLevel1Keys Level1Keys,
    AuthorizationsV0 Level2Keys);
