using System.Threading.Tasks;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetBlockSummaryAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetBlockSummaryAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_block_exists_should_return_correct_data()
    {
        // Arrange
        var blockHash = BlockHash.From("1895d3dfd287afc125927f46ae395718dcebcd9b98562903b0030968428f5179");

        // Act
        var blockSummary = await ConcordiumNodeClient.GetBlockSummaryAsync(blockHash);

        // Assert
        blockSummary.Should().NotBeNull();
    }
}
