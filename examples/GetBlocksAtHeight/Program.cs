using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBlocksAtHeight;

internal sealed class GetBlocksAtHeightOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlocksAtHeightAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlocksAtHeightOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlocksAtHeightOptions options)
    {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = new Uri(options.Endpoint)
        };
        using var client = new ConcordiumClient(clientOptions);

        var info = await client.GetConsensusInfoAsync();

        var absoluteHeight = new AbsoluteHeight(info.BestBlockHeight);

        var blocks = await client.GetBlocksAtHeightAsync(absoluteHeight, CancellationToken.None);

        Console.WriteLine($"Blocks live at height: {info.BestBlockHeight}");
        foreach (var block in blocks)
        {
            Console.WriteLine(block.ToString());
        }
    }
}
