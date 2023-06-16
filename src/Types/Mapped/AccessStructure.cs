using System.Collections.Immutable;

namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// And access structure for performing chain updates. The access structure is
/// only meaningful in the context of a list of update keys to which the indices
/// refer to.
/// </summary>
/// <param name="AuthorizedKeys"></param>
/// <param name="Threshold"></param>
public record AccessStructure(ImmutableHashSet<UpdateKeysIndex> AuthorizedKeys, UpdateKeysThreshold Threshold)
{
    internal static AccessStructure From(Grpc.V2.AccessStructure structure) =>
        new(
            structure.AccessPublicKeys.Select(UpdateKeysIndex.From).ToImmutableHashSet(),
            UpdateKeysThreshold.From(structure.AccessThreshold));
}
