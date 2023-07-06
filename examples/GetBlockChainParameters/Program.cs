using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBlockChainParameters;

internal sealed class GetBlockChainParametersOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlockChainParametersAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlockChainParametersOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlockChainParametersOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new ChainParameterSerializer() }
        };

        var block = new Absolute(42);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var chainParameters = await client.GetBlockChainParametersAsync(block, cts.Token);

        Console.WriteLine($"Block hash: {chainParameters.BlockHash}");
        Console.WriteLine($"Type of chain parameters: {chainParameters.Response.GetType()}");
        Console.WriteLine(JsonSerializer.Serialize(chainParameters.Response, jsonSerializerOptions));
        switch (chainParameters.Response)
        {
            case ChainParametersV0 v0:
                Console.WriteLine(JsonSerializer.Serialize(v0));
                break;
            case ChainParametersV1 chainParametersV1:
                break;
            case ChainParametersV2 chainParametersV2:
                break;
            default:
                throw new ArgumentOutOfRangeException(new($"unknown type: {chainParameters.Response.GetType()}"));
        }
    }
}

internal sealed class ChainParameterSerializer : JsonConverter<IChainParameters>
{
    public override IChainParameters? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IChainParameters value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ChainParametersV0 chainParametersV0:
                JsonSerializer.Serialize(writer, chainParametersV0, options);
                break;
            case ChainParametersV1 chainParametersV1:
                JsonSerializer.Serialize(writer, chainParametersV1, options);
                break;
            case ChainParametersV2 chainParametersV2:
                JsonSerializer.Serialize(writer, chainParametersV2, options);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
