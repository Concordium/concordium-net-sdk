using System.IO;
using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.AccountInfoResponse;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.CustomJsonSerializerTests;

public class AccountInfoTests
{
    // todo: think how to implement it for all response types and for diff json data that is stored in file
    [Theory]
    [InlineData("account-info1.json")]
    [InlineData("account-info2.json")]
    [InlineData("account-info3.json")]
    [InlineData("account-info4.json")]
    [InlineData("account-info5.json")]
    [InlineData("account-info6.json")]
    [InlineData("account-info7.json")]
    [InlineData("account-info8.json")]
    [InlineData("account-info9.json")]
    [InlineData("account-info10.json")]
    public async Task Should_correctly_deserialize_json_data(string path)
    {
        // Arrange
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, @$"CustomJsonSerializerTests/Files/{path}");
        var expectedJson = await File.ReadAllTextAsync(filePath);

        // Act
        var obj = CustomJsonSerializer.Deserialize<AccountInfo>(expectedJson);
        var actualJson = CustomJsonSerializer.Serialize(obj);
        var expected = JToken.Parse(expectedJson);
        var actual = JToken.Parse(actualJson);

        // Assert
        expected.Should().BeEquivalentTo(actual);
    }
}
