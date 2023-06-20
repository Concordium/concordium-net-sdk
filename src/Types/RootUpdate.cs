using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// An update with root keys of some other set of governance keys, or the root
/// keys themselves. Each update is a separate transaction.
/// </summary>
public interface IRootUpdate : IUpdatePayload {}

internal static class RootUpdateFactory
{
    internal static IRootUpdate From(Grpc.V2.RootUpdate root) =>
        root.UpdateTypeCase switch
        {
            RootUpdate.UpdateTypeOneofCase.RootKeysUpdate =>
                RootKeysUpdate.From(root.RootKeysUpdate),
            RootUpdate.UpdateTypeOneofCase.Level1KeysUpdate =>
                Level1KeysUpdate.From(root.Level1KeysUpdate),
            RootUpdate.UpdateTypeOneofCase.Level2KeysUpdateV0 =>
                Level2KeysUpdate.From(root.Level2KeysUpdateV0),
            RootUpdate.UpdateTypeOneofCase.Level2KeysUpdateV1 =>
                Level2KeysUpdateV1.From(root.Level2KeysUpdateV1),
            RootUpdate.UpdateTypeOneofCase.None =>
                throw new MissingEnumException<RootUpdate.UpdateTypeOneofCase>(root.UpdateTypeCase),
            _ => throw new MissingEnumException<RootUpdate.UpdateTypeOneofCase>(root.UpdateTypeCase)
        };
}

/// <summary>
/// Root key update.
/// </summary>
public sealed record RootKeysUpdate(HigherLevelKeys Keys) : IRootUpdate
{
    internal static RootKeysUpdate From(Grpc.V2.HigherLevelKeys keys) =>
        new( new RootKeys(
            keys.Keys.Select(UpdatePublicKey.From).ToArray(),
            UpdateKeysThreshold.From(keys.Threshold)
            ));
}

/// <summary>
/// Level 1 key update.
/// </summary>
public sealed record Level1KeysUpdate(HigherLevelKeys Keys) : IRootUpdate, ILevel1
{
    internal static Level1KeysUpdate From(Grpc.V2.HigherLevelKeys keys) =>
        new( new Level1Keys(
            keys.Keys.Select(UpdatePublicKey.From).ToArray(),
            UpdateKeysThreshold.From(keys.Threshold)
            ));
}

/// <summary>
/// Level 2 keys update with chain parameter version 0.
/// </summary>
/// <param name="AuthorizationsV0">
/// Access structures for each of the different possible chain updates, together
/// with the context giving all the possible keys.
/// </param>
public sealed record Level2KeysUpdate(AuthorizationsV0 AuthorizationsV0) : IRootUpdate, ILevel1
{
    internal static Level2KeysUpdate From(Grpc.V2.AuthorizationsV0 authorizationsV0) => new(AuthorizationsV0.From(authorizationsV0));
}

/// <summary>
/// Level 2 keys update with chain parameter version 1.
/// </summary>
/// <param name="AuthorizationsV1">
/// Access structures for each of the different possible chain updates, together
/// with the context giving all the possible keys.
/// </param>
public sealed record Level2KeysUpdateV1(AuthorizationsV1 AuthorizationsV1) : IRootUpdate, ILevel1
{
    internal static Level2KeysUpdateV1 From(Grpc.V2.AuthorizationsV1 authorizationsV1) => new(AuthorizationsV1.From(authorizationsV1));
}
