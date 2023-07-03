using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

#pragma warning disable CS8618

namespace Example;

internal sealed class GetBlocksAtHeightOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com/:20000")]
    public Uri Uri { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlocksAtHeightAsync"/>
    /// </summary>s
    public static async Task Main(string[] args)
    {
        await Parser.Default
            .ParseArguments<GetBlocksAtHeightOptions>(args)
            .WithParsedAsync(options => Run(options));
    }

    static async Task Run(GetBlocksAtHeightOptions options) {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var info = await client.GetConsensusInfoAsync();

        var absoluteHeight = new Absolute(info.BestBlockHeight);

        var blocks = await client.GetBlocksAtHeightAsync(absoluteHeight, CancellationToken.None);

        Console.WriteLine($"Blocks live at height: {info.BestBlockHeight}");
        foreach (var block in blocks)
        {
            Console.WriteLine(block.ToString());
        }
    }
}