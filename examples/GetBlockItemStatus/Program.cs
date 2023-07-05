using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

namespace GetBlockItemStatus;

internal sealed class GetBlockItemSummaryOptions
{
    [Option(HelpText = "Transaction hash to lookup", Required = true)]
    public string TransactionHash { get; set; }

    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com")]
    public string Endpoint { get; set; }

    [Option(HelpText = "Port for the gRPC V2 API.", Required = true, Default = 20_000)]
    public ushort Port { get; set; }
}

internal static class ExampleHelpers
{
    public static T Parse<T>(string[] args)
    {
        var parserResult = Parser.Default
            .ParseArguments<T>(args);
        if (parserResult.Errors.Any())
        {
            Environment.Exit(1);
        }
        return parserResult.Value;
    }
}

public static class Program
{
    /// <summary>
    /// An example showing how one can query transaction status from a node.
    /// </summary>
    /// <param name="args">GetBlockItemSummaryOptions
    /// Example: --endpoint http://node.testnet.concordium.com --transactionhash 143ca4183d0bb204000ad08e0fd5792985c808861b97f3b81cb9016ad39d09d2 --port 20000
    /// </param>
    public static async Task Main(string[] args)
    {
        var options = ExampleHelpers.Parse<GetBlockItemSummaryOptions>(args);

        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = new Uri($"{options!.Endpoint}:{options.Port}")
        };
        using var client = new ConcordiumClient(clientOptions);

        var transactionHash = TransactionHash.From(options.TransactionHash);

        Console.WriteLine("Query node...");
        var transactionStatus = await client.GetBlockItemStatusAsync(transactionHash);

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
            default:
                throw new NotImplementedException(transactionStatus.GetType().ToString());
        }
    }

    /// <summary>
    /// Example how one can handle received state.
    /// </summary>
    private static void HandleTransactionStatusReceived(TransactionStatusReceived _) => Console.WriteLine("Transaction received");

    /// <summary>
    /// Example how one can handle finalized state. One can use the BlockItemSummary to get additional information like
    /// rejected reason.
    /// </summary>
    private static void HandleTransactionStatusFinalized(TransactionStatusFinalized transactionStatusFinalized)
    {
        Console.WriteLine($"Finalized in block: {transactionStatusFinalized.State.BlockHash}");

        if (transactionStatusFinalized.State.Summary.TryGetRejectedAccountTransaction(out var rejectReason))
        {
            Console.WriteLine($"Transaction rejected due to: {rejectReason!.GetType().Name}");
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
