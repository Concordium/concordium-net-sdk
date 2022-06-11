using System.Linq;
using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.IpInfoResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetIdentityProvidersAsyncTests
{
    private ConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetIdentityProvidersAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_block_exists_should_return_correct_data()
    {
        // Arrange
        var emptyIpInfo = new IpInfo();
        var blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

        // Act
        var ipInfos = await ConcordiumNodeClient.GetIdentityProvidersAsync(blockHash);

        // Assert
        ipInfos.Should().NotBeEmpty();
        ipInfos.First().Should().NotBeEquivalentTo(emptyIpInfo);
    }
}
