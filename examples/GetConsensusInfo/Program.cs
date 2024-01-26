using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetConsensusInfo;

internal sealed class GetConsensusInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://grpc.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetConsensusInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetConsensusInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetConsensusInfoOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new BlockHashConverter() }
        };

        var poolStatus = await client.GetConsensusInfoAsync();

        var serialized = JsonSerializer.Serialize(poolStatus, jsonSerializerOptions);
        Console.WriteLine(serialized);
    }
}

internal sealed class BlockHashConverter : JsonConverter<BlockHash>
{
    public override BlockHash? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, BlockHash value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}
