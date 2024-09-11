using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types;

/// <summary>
/// A block identifier used in queries.
/// </summary>
public interface IBlockHashInput
{
    /// <summary>
    /// Convert this into the type expected by the gRPC API.
    /// </summary>
    BlockHashInput Into();
}

/// <summary>
/// Query in the context of the best block.
/// </summary>
public sealed record Best : IBlockHashInput
{
    private static readonly Empty _empty = new();

    /// <summary>
    /// Convert this into the type expected by the gRPC API.
    /// </summary>
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

    /// <summary>
    /// Convert this into the type expected by the gRPC API.
    /// </summary>
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
    /// <summary>
    /// Convert this into the type expected by the gRPC API.
    /// </summary>
    public BlockHashInput Into() => new() { Given = this.BlockHash.ToProto() };
}

/// <summary>
/// Block Height from beginning of chain.
/// </summary>
/// <param name="Height">Height from the beginning of the chain.</param>
public sealed record Absolute(ulong Height) : IBlockHashInput
{
    /// <summary>
    /// Query for a block at absolute height. If a unique
    /// block can not be identified at that height the query will
    /// throw an exception.
    /// </summary>
    BlockHashInput IBlockHashInput.Into() =>
        new()
        {
            AbsoluteHeight = new AbsoluteBlockHeight
            {
                Value = this.Height
            }
        };
}


/// <summary>
/// Query relative to an explicit genesis index.
/// </summary>
/// <param name="Height">Height starting from the genesis block at the genesis index.</param>
/// <param name="GenesisIndex">Genesis index to start from.</param>
/// <param name="Restrict">
/// Whether to return results only from the specified genesis index
/// (`true`), or allow results from more recent genesis indices
/// as well (`false`).
/// </param>
public sealed record Relative(ulong Height, uint GenesisIndex, bool Restrict) : IBlockHashInput
{
    /// <summary>
    /// Query for a block at a height relative to genesis index. If a unique
    /// block can not be identified at that height the query will
    /// throw an exception.
    /// </summary>
    BlockHashInput IBlockHashInput.Into() => new()
    {
        RelativeHeight = new BlockHashInput.Types.RelativeHeight
        {
            GenesisIndex = new GenesisIndex { Value = this.GenesisIndex },
            Height = new BlockHeight { Value = this.Height },
            Restrict = this.Restrict,
        }
    };
}
