using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;


namespace GetBakerList;

internal sealed class GetBakerListOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com:20000/")]
    public Uri Uri { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBakerListAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBakerListOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBakerListOptions options)
    {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var bakers = await client.GetBakerListAsync(new LastFinal());

        Console.WriteLine($"BlockHash: {bakers.BlockHash}");
        await foreach (var baker in bakers.Response)
        {
            Console.WriteLine($"Id: {baker}");
        }
    }
}
