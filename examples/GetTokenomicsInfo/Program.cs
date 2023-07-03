using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

#pragma warning disable CS8618

namespace GetTokenomicsInfo;

internal sealed class GetTokenomicsInfoOptions
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
    /// Example how to use <see cref="ConcordiumClient.GetTokenomicsInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetTokenomicsInfoOptions>(args)
            .WithParsedAsync(options => Run(options));

    static async Task Run(GetTokenomicsInfoOptions options) {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var block = BlockHash.From(options.BlockHash);

        var response = await client.GetTokenomicsInfoAsync(new Given(block));

        Console.WriteLine($"BlockHash: {response.BlockHash}");

        var rewardOverview = response.Response;
        Console.WriteLine($"Protocol version: {rewardOverview.ProtocolVersion}");

        switch (rewardOverview)
        {
            case RewardOverviewV0 rewardOverviewV0:
                Console.WriteLine("This is reward version 0 return type.");
                Console.WriteLine($"Total CCD in existence: {rewardOverviewV0.TotalAmount}");
                break;
            case RewardOverviewV1 rewardOverviewV1:
                Console.WriteLine("This is reward version 1 return type.");
                Console.WriteLine($"Next payday mint rate: {rewardOverviewV1.NextPaydayMintRate.AsDecimal()}");
                break;
            default:
                throw new ArgumentOutOfRangeException(new($"unknown type: {rewardOverview.GetType()}"));
        }
    }
}