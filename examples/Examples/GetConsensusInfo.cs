using System.Text.Json;
using System.Text.Json.Serialization;
using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetConsensusInfo : Tests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GetConsensusInfo(ITestOutputHelper output) : base(output) =>
        this._jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new BlockHashConverter() }
        };

    [Fact]
    public async Task RunGetConsensusInfoAsync()
    {
        var poolStatus = await this.Client.GetConsensusInfoAsync();

        var serialized = JsonSerializer.Serialize(poolStatus, this._jsonSerializerOptions);
        this.Output.WriteLine(serialized);
    }
}

internal sealed class BlockHashConverter : JsonConverter<BlockHash>
{
    public override BlockHash? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, BlockHash value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}
