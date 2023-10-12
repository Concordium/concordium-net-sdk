using System.Text.Json;
using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetInstanceInfo;

internal sealed class GetInstanceInfoOptions
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
        "contract-address-index",
        HelpText = "Contract address index from where module source should be loaded.",
        Required = true
    )]
    public ulong Index { get; set; }

    [Option(
        's',
        "contract-address-subindex",
        HelpText = "Contract address subindex from where module source should be loaded.",
        Required = true
    )]
    public ulong Subindex { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetInstanceInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetInstanceInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetInstanceInfoOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var block = BlockHash.From(options.BlockHash);
        var contractAddress = ContractAddress.From(options.Index, options.Subindex);

        var queryResponse = await client.GetInstanceInfoAsync(new Given(block), contractAddress);

        var jsonSerializerOptions = new JsonSerializerOptions
        {

            WriteIndented = true
        };
        var info = queryResponse.Response switch
        {
            InstanceInfoV0 v0 => JsonSerializer.Serialize(v0, jsonSerializerOptions),
            InstanceInfoV1 v1 => JsonSerializer.Serialize(v1, jsonSerializerOptions),
            _ => throw new ArgumentException($"unknown type {queryResponse.Response.GetType()}")
        };

        Console.WriteLine(info);
    }
}
