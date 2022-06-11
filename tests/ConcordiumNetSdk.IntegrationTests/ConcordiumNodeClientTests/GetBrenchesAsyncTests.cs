using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.BranchResponse;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetBrenchesAsyncTests
{
    private ConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetBrenchesAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task Should_return_correct_data()
    {
        // Arrange
        // todo: think of better implementation of empty object
        var emptyBranch = new Branch();
        
        // Act
        var branch = await ConcordiumNodeClient.GetBranchesAsync();

        // Assert
        branch.Should().NotBeNull();
        branch.Should().NotBeEquivalentTo(emptyBranch);
    }
}
