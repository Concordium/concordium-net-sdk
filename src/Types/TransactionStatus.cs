using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// A common return type for TransactionStatus.
/// </summary>
public interface ITransactionStatus
{ }

/// <summary>
/// Transaction is received, but not yet in any blocks.
/// </summary>
public sealed record TransactionStatusReceived : ITransactionStatus
{ }

/// <summary>
/// Transaction is finalized in the given block, with the given summary.
/// </summary>
public sealed record TransactionStatusFinalized : ITransactionStatus
{
    /// <summary>
    /// Record with the block hash and summary of the outcome of a block item.
    /// </summary>
    public (BlockHash BlockHash, BlockItemSummary Summary) State { get; }

    internal TransactionStatusFinalized(BlockItemStatus blockItemStatus)
    {
        var blockItemSummaryInBlock = blockItemStatus.Finalized.Outcome;
        var itemSummary = blockItemSummaryInBlock.Outcome;

        var blockItemSummary = BlockItemSummary.From(itemSummary);
        this.State = (BlockHash.From(blockItemSummaryInBlock.BlockHash.Value), blockItemSummary);
    }
}

/// <summary>
/// Transaction is committed to one or more blocks. The outcomes are listed
/// for each block. Note that in the vast majority of cases the outcome of a
/// transaction should not be dependent on the block it is in, but this
/// can in principle happen.
/// </summary>
public sealed record TransactionStatusCommitted : ITransactionStatus
{
    /// <summary>
    /// Map with records which each gives the block hash and summary of the outcome of a block item.
    /// </summary>
    public IList<(BlockHash BlockHash, BlockItemSummary Summary)> States { get; }

    internal TransactionStatusCommitted(BlockItemStatus blockItemStatus)
    {
        this.States = new List<(BlockHash, BlockItemSummary)>();
        foreach (var blockItemSummaryInBlock in blockItemStatus.Committed.Outcomes)
        {
            var itemSummary = blockItemSummaryInBlock.Outcome;
            var blockItemSummary = BlockItemSummary.From(itemSummary);
            var hash = BlockHash.From(blockItemSummaryInBlock.BlockHash.Value);
            var state = (hash, blockItemSummary);

            this.States.Add(state);
        }
    }
}

/// <summary>
/// Creates a TransactionStatus dependent of the status in the returned BlockItemStatus from the node.
/// </summary>
internal static class TransactionStatusFactory
{
    internal static ITransactionStatus CreateTransactionStatus(BlockItemStatus blockItemStatus) =>
        blockItemStatus.StatusCase switch
        {
            BlockItemStatus.StatusOneofCase.Received => new TransactionStatusReceived(),
            BlockItemStatus.StatusOneofCase.Committed => new TransactionStatusCommitted(blockItemStatus),
            BlockItemStatus.StatusOneofCase.Finalized => new TransactionStatusFinalized(blockItemStatus),
            BlockItemStatus.StatusOneofCase.None => throw new MissingEnumException<BlockItemStatus.StatusOneofCase>(blockItemStatus.StatusCase),
            _ => throw new MissingEnumException<BlockItemStatus.StatusOneofCase>(blockItemStatus.StatusCase)
        };
}
