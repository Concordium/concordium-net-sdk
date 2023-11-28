using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBlockItems;

internal sealed class GetBlocksOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block. Defaults to LastFinal."
    )]
    public string BlockHash { get; set; }
}

public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlockItems"/>
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlocksOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlocksOptions o)
    {
        using var client = new ConcordiumClient(new Uri(o.Endpoint), new ConcordiumClientOptions());

        IBlockHashInput bi = o.BlockHash != null ? new Given(BlockHash.From(o.BlockHash)) : new LastFinal();

        var blockItems = client.GetBlockItems(bi);

        await foreach (var item in blockItems)
        {
            Console.WriteLine($"Blockitem: {item}");
        }
    }
}

