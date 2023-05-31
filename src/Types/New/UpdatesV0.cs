namespace Concordium.Sdk.Types.New;

public record UpdatesV0(
    UpdateKeysCollectionV0 Keys,
    ProtocolUpdate? ProtocolUpdate,
    ChainParametersV0 ChainParameters,
    PendingUpdatesV0 UpdateQueues);
