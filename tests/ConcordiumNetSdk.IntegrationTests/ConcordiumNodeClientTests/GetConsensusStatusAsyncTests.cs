using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.ConsensusStatusResponse;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetConsensusStatusAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetConsensusStatusAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_should_return_correct_data()
    {
        // Arrange
        var emptyConsensusStatus = new ConsensusStatus();
        
        // Act
        var consensusStatus = await ConcordiumNodeClient.GetConsensusStatusAsync();

        // Assert
        consensusStatus.Should().NotBeNull();
        consensusStatus.Should().NotBe(emptyConsensusStatus);
    }
}
