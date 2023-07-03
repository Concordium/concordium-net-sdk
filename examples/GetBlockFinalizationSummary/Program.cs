using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

#pragma warning disable CS8618

namespace GetBlockFinalizationSummary;

internal sealed class GetBlockFinalizationSummaryOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com:20000/")]
    public Uri Uri { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlockFinalizationSummary"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlockFinalizationSummaryOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlockFinalizationSummaryOptions options)
    {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var idx = 0UL;
        var awaitResult = true;
        while (awaitResult)
        {
            var blockHeight = new Absolute(idx);
            var response = await client.GetBlockFinalizationSummaryAsync(blockHeight);
            var finalizationSummary = response.Response;
            if (finalizationSummary == null)
            {
                idx++;
                continue;
            }
            awaitResult = false;

            Console.WriteLine($"At height {idx} block {finalizationSummary.BlockPointer} had finalization summary");
            Console.WriteLine($"Finalization round index: {finalizationSummary.Index}, finalization delay: {finalizationSummary.Delay}");
            Console.WriteLine($"With finalizers");
            foreach (var party in finalizationSummary.Finalizers)
            {
                Console.WriteLine($"Baker: {party.BakerId}, weight in committee: {party.Weight} with signature present: {party.SignaturePresent}");
            }
        }
    }
}
