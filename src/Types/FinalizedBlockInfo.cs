namespace Concordium.Sdk.Types;

/// <summary>
/// Information about a finalized block that is part of the streaming response.
/// </summary>
/// <param name="BlockHash">Hash of the block.</param>
/// <param name="BlockHeight">Absolute height of the block, height 0 is the genesis block.</param>
public sealed record FinalizedBlockInfo(BlockHash BlockHash, ulong BlockHeight)
{
    internal static FinalizedBlockInfo From(Grpc.V2.FinalizedBlockInfo info) =>
        new(
            BlockHash.From(info.Hash),
            info.Height.Value
        );
}
