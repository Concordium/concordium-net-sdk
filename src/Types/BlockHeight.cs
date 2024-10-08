using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types;

/// <summary>
/// Interface which covers block heights and can be mapped into gRPC block height request types.
/// </summary>
public interface IBlockHeight
{
    /// <summary>
    /// Convert into a BlocksAtHeightRequest.
    /// </summary>
    BlocksAtHeightRequest Into();
}

/// <summary>
/// Block Height from beginning of chain.
/// </summary>
/// <param name="Height">Height from the beginning of the chain.</param>
public sealed record AbsoluteHeight(ulong Height) : IBlockHeight
{
    internal static AbsoluteHeight From(AbsoluteBlockHeight blockHeight) => new(blockHeight.Value);

    /// <summary>
    /// Convert an AbsoluteHeight into a BlocksAtHeightRequest.
    /// </summary>
    public BlocksAtHeightRequest Into() =>
        new()
        {
            Absolute = new BlocksAtHeightRequest.Types.Absolute
            {
                Height = new AbsoluteBlockHeight { Value = this.Height }
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
public sealed record RelativeHeight(ulong Height, uint GenesisIndex, bool Restrict) : IBlockHeight
{
    /// <summary>
    /// Convert a RelativeHeight into a BlocksAtHeightRequest.
    /// </summary>
    public BlocksAtHeightRequest Into() =>
        new()
        {
            Relative = new BlocksAtHeightRequest.Types.Relative
            {
                GenesisIndex = new GenesisIndex { Value = this.GenesisIndex },
                Height = new BlockHeight { Value = this.Height },
                Restrict = this.Restrict,
            }
        };
}
