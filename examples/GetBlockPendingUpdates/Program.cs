using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBlockPendingUpdates;

internal sealed class GetBlockPendingUpdatesOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block."
    )]
    public string BlockHash { get; set; }
}

public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlockPendingUpdates"/>
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlockPendingUpdatesOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlockPendingUpdatesOptions o)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        using var client = new ConcordiumClient(new Uri(o.Endpoint), new ConcordiumClientOptions());

        IBlockHashInput bi = o.BlockHash != null ? new Given(BlockHash.From(o.BlockHash)) : new LastFinal();

        var updates = await client.GetBlockPendingUpdates(bi);

        Console.WriteLine($"Updates:");
        await foreach (var update in updates.Response)
        {
            Console.WriteLine($"Pending update: {update}");

        }
    }
}
