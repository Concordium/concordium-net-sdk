using CommandLine;
using Concordium.Sdk.Client;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetNodeInfo;

internal sealed class GetNodeInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://grpc.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetNodeInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetNodeInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetNodeInfoOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var peerVersion = await client.GetNodeInfoAsync();

        Console.WriteLine($"Version of node was: {peerVersion.Version}");
    }
}
