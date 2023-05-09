using Concordium.Grpc.V2;
using Google.Protobuf;

namespace BlockItemSummary;

public interface ITransactionStatus
{}

public sealed class TransactionStatusReceived : ITransactionStatus
{}

public sealed class TransactionStatusFinalized : ITransactionStatus
{
    private readonly (byte[], BlockItemSummary) state;

    internal TransactionStatusFinalized(BlockItemStatus blockItemStatus)
    {
        var blockItemSummaryInBlock = blockItemStatus.Finalized.Outcome;
        // TODO remove
        Console.WriteLine(blockItemSummaryInBlock.BlockHash);
        var itemSummary = blockItemSummaryInBlock.Outcome;
        var blockItemSummary = new BlockItemSummary(itemSummary);
        var hash = blockItemSummaryInBlock.BlockHash.ToByteArray()!;
        state = (hash, blockItemSummary);
    }
}

public sealed class TransactionStatusCommitted : ITransactionStatus
{
    private readonly IList<(byte[], BlockItemSummary)> states;
    
    internal TransactionStatusCommitted(BlockItemStatus blockItemStatus)
    {
        states = new List<(byte[], BlockItemSummary)>();
        foreach (var blockItemSummaryInBlock in blockItemStatus.Committed.Outcomes)
        {
            var itemSummary = blockItemSummaryInBlock.Outcome;
            var blockItemSummary = new BlockItemSummary(itemSummary);
            var hash = blockItemSummaryInBlock.BlockHash.ToByteArray()!;
        
            var state = (hash, blockItemSummary);
            states.Add(state);
        }
    }
}   

internal static class TransactionStatusFactory
{
    internal static ITransactionStatus CreateTransactionStatus(BlockItemStatus blockItemStatus)
    {
        return blockItemStatus.StatusCase switch
        {
            BlockItemStatus.StatusOneofCase.Received => new TransactionStatusReceived(),
            BlockItemStatus.StatusOneofCase.Committed => new TransactionStatusCommitted(blockItemStatus),
            BlockItemStatus.StatusOneofCase.Finalized => new TransactionStatusFinalized(blockItemStatus),
            _ => throw new TransactionStatusException(blockItemStatus)
        };
    }
}

public class TransactionStatusException : Exception {
    internal TransactionStatusException(BlockItemStatus blockItemStatus) 
        : base($"BlockItemStatus returned {blockItemStatus.StatusCase}") {}
}