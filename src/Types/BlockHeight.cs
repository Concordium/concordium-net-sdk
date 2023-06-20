using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types;

/// <summary>
/// Interface which covers block heights and can be mapped into gRPC block height request types.
/// </summary>
public interface IBlockHeight
{
    BlocksAtHeightRequest Into();
}

/// <summary>
/// Block Height from beginning of chain.
/// </summary>
/// <param name="Height">Height from the beginning of the chain.</param>
public sealed record Absolute(ulong Height) : IBlockHeight, IBlockHashInput
{
    public BlocksAtHeightRequest Into() =>
        new()
        {
            Absolute = new BlocksAtHeightRequest.Types.Absolute
            {
                Height = new AbsoluteBlockHeight { Value = this.Height }
            }
        };

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
public sealed record Relative(ulong Height, uint GenesisIndex, bool Restrict) : IBlockHeight, IBlockHashInput
{
    public BlocksAtHeightRequest Into() =>
        new()
        {
            Relative = new BlocksAtHeightRequest.Types.Relative
            {
                GenesisIndex = new GenesisIndex{Value = this.GenesisIndex},
                Height = new BlockHeight{Value = this.Height},
                Restrict = this.Restrict,
            }
        };

    /// <summary>
    /// Query for a block at a height relative to genesis index. If a unique
    /// block can not be identified at that height the query will
    /// throw an exception.
    /// </summary>
    BlockHashInput IBlockHashInput.Into() => new BlockHashInput
    {
        RelativeHeight = new BlockHashInput.Types.RelativeHeight
        {
            GenesisIndex = new GenesisIndex{Value = this.GenesisIndex},
            Height = new BlockHeight{Value = this.Height},
            Restrict = this.Restrict,
        }
    };
}
