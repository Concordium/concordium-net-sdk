using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.BirkParametersResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetBirkParametersAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetBirkParametersAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_block_exists_should_return_correct_data()
    {
        // Arrange
        var emptyBirkParameters = new BirkParameters();
        var blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

        // Act
        var birkParameters = await ConcordiumNodeClient.GetBirkParametersAsync(blockHash);

        // Assert
        birkParameters.Should().NotBeNull();
        birkParameters.Should().NotBeEquivalentTo(emptyBirkParameters);
    }
}
