using CommandLine;
using Concordium.Sdk.Client;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBlocks;

internal sealed class GetBlocksOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}

public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlocks"/>
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlocksOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlocksOptions options)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var blocks = client.GetBlocks();

        await foreach (var block in blocks)
        {
            Console.WriteLine($"Block arrived: {block}");
        }
    }
}
