using CommandLine;
using Concordium.Sdk.Client;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetFinalizedBlocks;

internal sealed class GetFinalizedBlocksOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}

public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetFinalizedBlocks"/>
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetFinalizedBlocksOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetFinalizedBlocksOptions options)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var blocks = client.GetFinalizedBlocks();

        await foreach (var block in blocks)
        {
            Console.WriteLine($"Finalized block arrived: {block}");
        }
    }
}
