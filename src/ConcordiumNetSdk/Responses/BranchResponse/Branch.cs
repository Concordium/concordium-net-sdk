using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.BranchResponse;

/// <summary>
/// Represents branches of the tree from the last finalized block as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetBranchesAsync"/>.
/// See <a href="https://github.com/Concordium/concordium-node/edit/main/docs/grpc.md#getbranches--branch">here</a>.
/// </summary>
public record Branch
{
    /// <summary>
    /// Gets or initiates the hash of the block (base 16 encoded).
    /// </summary>
    public BlockHash BlockHash { get; init; }

    /// <summary>
    /// Gets or initiates the list of JSON objects encoding the children of the block, similarly encoded.
    /// </summary>
    public List<Branch> Children { get; init; }
}
