namespace Concordium.Sdk.Types;

/// <summary>
/// Information about an arrived block that is part of the streaming response.
/// </summary>
/// <param name="BlockHash">Hash of the block.</param>
/// <param name="BlockHeight">Absolute height of the block, height 0 is the genesis block.</param>
public sealed record ArrivedBlockInfo(BlockHash BlockHash, ulong BlockHeight)
{
    internal static ArrivedBlockInfo From(Grpc.V2.ArrivedBlockInfo info) =>
        new(
            BlockHash.From(info.Hash),
            info.Height.Value
        );
}
