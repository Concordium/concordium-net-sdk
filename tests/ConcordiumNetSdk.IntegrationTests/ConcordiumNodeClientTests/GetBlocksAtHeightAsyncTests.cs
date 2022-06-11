using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetBlocksAtHeightAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetBlocksAtHeightAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task Should_return_correct_data()
    {
        // Arrange
        var blockHeight = 10u;

        // Act
        var blocks = await ConcordiumNodeClient.GetBlocksAtHeightAsync(blockHeight);

        // Assert
        blocks.Should().NotBeEmpty();
    }
}
