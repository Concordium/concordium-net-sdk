using System.Threading.Tasks;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetAncestorsAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetAncestorsAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_block_exists_should_return_correct_data()
    {
        // Arrange
        var amount = 10ul;
        var blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

        // Act
        var blocks = await ConcordiumNodeClient.GetAncestorsAsync(amount, blockHash);

        // Assert
        blocks.Count.Should().Be((int)amount);
    }
}
