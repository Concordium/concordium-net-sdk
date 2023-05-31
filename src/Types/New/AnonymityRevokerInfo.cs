namespace Concordium.Sdk.Types.New;

public record AnonymityRevokerInfo(
    uint ArIdentity,
    ArOrIpDescription ArDescription,
    string ArPublicKey);