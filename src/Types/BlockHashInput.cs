using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types;

/// <summary>
/// A block identifier used in queries.
/// </summary>
public interface IBlockHashInput
{
    BlockHashInput Into();
}

/// <summary>
/// Query in the context of the best block.
/// </summary>
public sealed record Best : IBlockHashInput
{
    private static readonly Empty _empty = new();

    public BlockHashInput Into() => new()
    {
        Best = _empty,
    };
}

/// <summary>
/// Query in the context of the last finalized block at the time of the
/// query.
/// </summary>
public sealed record LastFinal : IBlockHashInput
{
    private static readonly Empty _empty = new();

    public BlockHashInput Into() => new()
    {
        LastFinal = _empty
    };
}

/// <summary>
/// Query in the context of a specific block hash.
/// </summary>
public sealed record Given(BlockHash BlockHash) : IBlockHashInput
{
    public BlockHashInput Into() => new() { Given = this.BlockHash.ToProto() };
}
