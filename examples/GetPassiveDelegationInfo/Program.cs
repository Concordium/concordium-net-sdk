using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;


namespace GetPassiveDelegationInfo;

internal sealed class GetPassiveDelegationInfoOptions
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
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetPassiveDelegationInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetPassiveDelegationInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetPassiveDelegationInfoOptions options)
    {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var block = BlockHash.From(options.BlockHash);

        var response = await client.GetPassiveDelegationInfoAsync(new Given(block));

        Console.WriteLine($"BlockHash: {response.BlockHash}");

        var poolStatus = response.Response;
        Console.WriteLine("The current commission rates are:");
        Console.WriteLine($"Baking Commission: {poolStatus.CommissionRates.BakingCommission}");
        Console.WriteLine($"Finalization Commission: {poolStatus.CommissionRates.FinalizationCommission}");
        Console.WriteLine($"Transaction Commission: {poolStatus.CommissionRates.TransactionCommission}");
    }
}
