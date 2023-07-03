using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

#pragma warning disable CS8618

namespace GetIdentityProviders;

internal sealed class GetIdentityProvidersOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com:20000/")]
    public Uri Uri { get; set; }
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

    private static async Task Run(GetIdentityProvidersOptions options) {
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

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
