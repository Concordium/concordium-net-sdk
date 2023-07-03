using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

#pragma warning disable CS8618

namespace GetPoolInfo;

internal sealed class GetPoolInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com:20000/")]
    public Uri Uri { get; set; }

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
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var block = BlockHash.From(options.BlockHash);
        var bakerId = new BakerId(new AccountIndex(options.BakerId));

        var response = await client.GetPoolInfoAsync(bakerId, new Given(block));

        Console.WriteLine($"BlockHash: {response.BlockHash}");

        Console.WriteLine($"Baker {bakerId} has baker equity capital {response.Response.BakerEquityCapital.GetFormattedCcd()}");
    }
}