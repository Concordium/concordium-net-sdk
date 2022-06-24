using System.IO;
using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.BlockSummaryResponse;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.CustomJsonSerializerTests;

public class BlockSummaryTests
{
    // todo: think how to implement it for all response types and for diff json data that is stored in file
    [Fact]
    public async Task Should_correctly_deserialize_json_data()
    {
        // Arrange
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, @"CustomJsonSerializerTests/Files/block-summary.json");
        var expectedJson = await File.ReadAllTextAsync(filePath);

        // Act
        var obj = CustomJsonSerializer.Deserialize<BlockSummary>(expectedJson);
        var actualJson = CustomJsonSerializer.Serialize(obj);
        var expected = JToken.Parse(expectedJson);
        var actual = JToken.Parse(actualJson);

        // Assert
        expected.Should().BeEquivalentTo(actual);
    }
}
