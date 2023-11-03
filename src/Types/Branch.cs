namespace Concordium.Sdk.Types;

/// <summary>
/// Response type for GetBranches.
/// </summary>
/// <param name="BlockHash">The hash of the block.</param>
/// <param name="Children">Further blocks branching of this block.</param>
public sealed record Branch(BlockHash BlockHash, List<Branch> Children)
{
    internal static Branch From(Grpc.V2.Branch info) =>
        new(
            BlockHash.From(info.BlockHash),
            info.Children.Select(From).ToList()
        );
}
