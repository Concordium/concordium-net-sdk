using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;


namespace GetBlockSpecialEvents;

internal sealed class GetBlockSpecialEventsOptions
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
    /// Example how to use <see cref="ConcordiumClient.GetBlockSpecialEvents"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlockSpecialEventsOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlockSpecialEventsOptions options)
    {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var block = BlockHash.From(options.BlockHash);
        var response = await client.GetBlockSpecialEvents(new Given(block));

        Console.WriteLine($"BlockHash: {response.BlockHash}");
        await foreach (var specialEvent in response.Response)
        {
            Console.WriteLine($"Type of special event is: {specialEvent.GetType().Name}");
        }
    }
}
