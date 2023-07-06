namespace Concordium.Sdk.Types;

/// <summary>
/// Identity of the anonymity revoker on the chain. This defines their
/// evaluation point for secret sharing, and thus it cannot be 0.
/// </summary>
public readonly record struct ArIdentity(uint Id)
{
    internal static ArIdentity From(Grpc.V2.ArInfo.Types.ArIdentity info) => new(info.Value);
}
