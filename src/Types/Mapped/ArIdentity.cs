namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Identity of the anonymity revoker on the chain. This defines their
/// evaluation point for secret sharing, and thus it cannot be 0.
/// </summary>
public record struct ArIdentity(uint Id)
{
    internal static ArIdentity From(Grpc.V2.ArInfo.Types.ArIdentity info) => new(info.Value);
}
