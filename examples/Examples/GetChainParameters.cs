using System.Text.Json;
using System.Text.Json.Serialization;
using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetChainParameters : Tests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GetChainParameters(ITestOutputHelper output) : base(output) =>
        this._jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new ChainParameterSerializer() }
        };

    [Fact]
    public async Task RunGetChainParametersAsync()
    {
        var block = new Absolute(42);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var chainParameters = await this.Client.GetBlockChainParametersAsync(block, cts.Token);

        this.Output.WriteLine($"Block hash: {chainParameters.BlockHash}");
        this.Output.WriteLine($"Type of chain parameters: {chainParameters.Response.GetType()}");
        this.Output.WriteLine(JsonSerializer.Serialize(chainParameters.Response, this._jsonSerializerOptions));
        switch (chainParameters.Response)
        {
            case ChainParametersV0 v0:
                this.Output.WriteLine(JsonSerializer.Serialize(v0));
            break;
            case ChainParametersV1 chainParametersV1:
                break;
            case ChainParametersV2 chainParametersV2:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(chainParameters));
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
