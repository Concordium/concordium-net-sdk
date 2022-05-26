using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.BlockInfoResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetBlockInfoAsyncTests
{
    private ConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetBlockInfoAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_block_exists_should_return_correct_data()
    {
        // Arrange
        // todo: think of better implementation of empty object
        var emptyBlockInfo = new BlockInfo();
        var blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

        // Act
        var blockInfo = await ConcordiumNodeClient.GetBlockInfoAsync(blockHash);

        // Assert
        blockInfo.Should().NotBeNull();
        blockInfo.Should().NotBeEquivalentTo(emptyBlockInfo);
    }
}
