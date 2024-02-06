using System.Text.Json;
using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBlockTransactionEvents;

internal sealed class GetBlockTransactionEventsOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://grpc.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlockTransactionEvents"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBlockTransactionEventsOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBlockTransactionEventsOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new BlockItemSummaryDetailsConverter(), new UpdatePayloadConverter() }
        };

        var idx = 0UL;
        var transactionCount = 0;
        while (transactionCount < 2)
        {
            var blockHeight = new Absolute(idx);
            var response = await client.GetBlockTransactionEvents(blockHeight);

            Console.WriteLine($"BlockHash: {response.BlockHash}");
            await foreach (var transaction in response.Response)
            {
                transactionCount++;
                var serialized = JsonSerializer.Serialize(transaction, jsonSerializerOptions);
                Console.WriteLine(serialized);
            }
            idx++;
        }
    }
}

internal sealed class BlockItemSummaryDetailsConverter : System.Text.Json.Serialization.JsonConverter<IBlockItemSummaryDetails>
{
    public override IBlockItemSummaryDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IBlockItemSummaryDetails value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}

internal sealed class UpdatePayloadConverter : System.Text.Json.Serialization.JsonConverter<IUpdatePayload>
{
    public override IUpdatePayload? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IUpdatePayload value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
