using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetModuleSource;

internal sealed class GetModuleSourceOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }

    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block.",
        Required = true
    )]
    public string BlockHash { get; set; }

    [Option(
        'i',
        "module-ref",
        HelpText = "Module reference in hexadecimal from where module source should be loaded.",
        Required = true
    )]
    public string ModuleReference { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetModuleSourceAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetModuleSourceOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetModuleSourceOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var block = BlockHash.From(options.BlockHash);
        var moduleReference = new ModuleReference(options.ModuleReference);

        var queryResponse = await client.GetModuleSourceAsync(new Given(block), moduleReference);

        Console.WriteLine($"Module source: {Convert.ToHexString(queryResponse.Response.Source)}");
    }
}
