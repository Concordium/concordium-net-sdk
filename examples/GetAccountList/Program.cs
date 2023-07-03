using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

#pragma warning disable CS8618

namespace GetAccountList;

internal sealed class GetAccountListOptions
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
    /// Example how to use <see cref="ConcordiumClient.GetAccountListAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetAccountListOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetAccountListOptions options)
    {
        var block = BlockHash.From(options.BlockHash);
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var response = await client.GetAccountListAsync(new Given(block));

        Console.WriteLine($"BlockHash: {response.BlockHash}");
        await foreach (var account in response.Response)
        {
            Console.WriteLine($"Account: {account}");
        }
    }
}