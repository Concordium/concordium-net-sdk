using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

namespace BlockItemSummary;

public static class Program
{
    /// <summary>
    /// An example showing how one can query transaction status from a node.
    /// </summary>
    /// <param name="args">endpoint transactionHash
    /// Example: http://node.testnet.concordium.com/ 6bd564e9e600fd0e5b0e197133a3448c819dfd4f9bcbec956a31d5b2a5029c48</param>
    public static async Task Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: .. endpoint transactionHash");
            return;
        }

        var endpoint = new Uri(args[0]);
        var transactionHash = TransactionHash.From(args[1]);

        using var client = new ConcordiumClient(endpoint, 20_000);

        Console.WriteLine("Query node...");
        var transactionStatus = await client.GetBlockItemStatus(transactionHash);

        // Conditional on the transaction status different actions can be handled.
        switch (transactionStatus)
        {
            case TransactionStatusReceived received:
                HandleTransactionStatusReceived(received);
                break;
            case TransactionStatusFinalized finalized:
                HandleTransactionStatusFinalized(finalized);
                break;
            case TransactionStatusCommitted committed:
                HandleTransactionStatusCommitted(committed);
                break;
        };
    }

    /// <summary>
    /// Example how one can handle received state.
    /// </summary>
    private static void HandleTransactionStatusReceived(TransactionStatusReceived transactionStatusReceived) => Console.WriteLine("Transaction received");

    /// <summary>
    /// Example how one can handle finalized state. One can use the BlockItemSummary to get additional information like
    /// rejected reason.
    /// </summary>
    private static void HandleTransactionStatusFinalized(TransactionStatusFinalized transactionStatusFinalized)
    {
        Console.WriteLine($"Finalized in block: {transactionStatusFinalized.State.BlockHash}");

        if (transactionStatusFinalized.State.Summary.IsRejectedAccountTransaction(out var rejectReason))
        {
            Console.WriteLine($"Transaction rejected due to: {rejectReason!.ReasonCase.ToString()}");
        }
    }

    /// <summary>
    /// Example how one can handle committed state. The transaction can be in several blocks and those are given
    /// in the State property as a map of BlockItemSummaries.
    /// From the BlockItemSummary one can ex. query affected addresses.
    /// </summary>
    private static void HandleTransactionStatusCommitted(TransactionStatusCommitted transactionStatusCommitted)
    {
        foreach (var (blockHash, summary) in transactionStatusCommitted.States)
        {
            Console.WriteLine($"Committed in block: {blockHash}");

            Console.WriteLine("List affected addresses in block:");
            foreach (var affectedAddress in summary.AffectedAddresses())
            {
                Console.WriteLine(affectedAddress);
            }
        }
    }
}
