using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetAncestors;

internal sealed class GetAncestorsOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://grpc.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
    [Option(
        'm',
        "max-ancestors",
        HelpText = "The maximum number of ancestors returned.",
        Required = true
    )]
    public ulong MaxAncestors { get; set; }
    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block. Defaults to LastFinal."
    )]
    public string BlockHash { get; set; }
}

public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetAncestors"/>
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetAncestorsOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetAncestorsOptions o)
    {
        using var client = new ConcordiumClient(new Uri(o.Endpoint), new ConcordiumClientOptions());

        IBlockHashInput bi = o.BlockHash != null ? new Given(BlockHash.From(o.BlockHash)) : new LastFinal();

        var ancestors = await client.GetAncestors(bi, o.MaxAncestors);

        await foreach (var ancestor in ancestors.Response)
        {
            Console.WriteLine($"Ancestor: {ancestor}");
        }
    }
}
