using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetIdentityProviders;

internal sealed class GetIdentityProvidersOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://grpc.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetIdentityProvidersAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetIdentityProvidersOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetIdentityProvidersOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var identityProviders = await client.GetIdentityProvidersAsync(new LastFinal());

        Console.WriteLine($"BlockHash: {identityProviders.BlockHash}");
        await foreach (var info in identityProviders.Response)
        {
            Console.WriteLine($"Id: {info.IpIdentity.Id}");
            Console.WriteLine($"Description info: {info.Description.Info}");
            Console.WriteLine($"Description name: {info.Description.Name}");
            Console.WriteLine($"Description url: {info.Description.Url}");
        }
    }
}
