using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetBannedPeersAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetBannedPeersAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task Should_return_correct_data()
    {
        // Arrange

        // Act
        var peerListResponse = await ConcordiumNodeClient.GetBannedPeersAsync();

        // Assert
        peerListResponse.Should().NotBeNull();
    }
}
