using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

#pragma warning disable CS8618

namespace Example;

internal sealed class GetBlockInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com/:20000")]
    public Uri Uri { get; set; }

    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block.",
        Required = true
    )]
    public string BlockHash { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlockInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args)
    {
        await Parser.Default
            .ParseArguments<GetBlockInfoOptions>(args)
            .WithParsedAsync(options => Run(options));
    }

    static async Task Run(GetBlockInfoOptions options) {
        var block = BlockHash.From(options.BlockHash);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var response = await client.GetBlockInfoAsync(new Given(block), cts.Token);

        Console.WriteLine($"Block: {response.BlockHash} has transaction energy cost: {response.Response.TransactionEnergyCost}");
    }
}