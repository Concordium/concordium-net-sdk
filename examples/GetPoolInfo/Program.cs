using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetPoolInfo;

internal sealed class GetPoolInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }

    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block.",
        Required = true
    )]
    public string BlockHash { get; set; }

    [Option(
        "baker-id",
        HelpText = "Baker Id",
        Required = true
    )]
    public ulong BakerId { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetPoolInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetPoolInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetPoolInfoOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var block = BlockHash.From(options.BlockHash);
        var bakerId = new BakerId(new AccountIndex(options.BakerId));

        var response = await client.GetPoolInfoAsync(bakerId, new Given(block));

        Console.WriteLine($"BlockHash: {response.BlockHash}");

        Console.WriteLine($"Baker {bakerId} has baker equity capital {response.Response.BakerEquityCapital.GetFormattedCcd()}");
    }
}
