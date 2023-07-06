using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618
namespace GetTokenomicsInfo;

internal sealed class GetTokenomicsInfoOptions
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
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetTokenomicsInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetTokenomicsInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetTokenomicsInfoOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

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
                Console.WriteLine($"Next payday mint rate: {rewardOverviewV1.NextPaydayMintRate.GetValues()}");
                break;
            default:
                throw new ArgumentOutOfRangeException(new($"unknown type: {rewardOverview.GetType()}"));
        }
    }
}
