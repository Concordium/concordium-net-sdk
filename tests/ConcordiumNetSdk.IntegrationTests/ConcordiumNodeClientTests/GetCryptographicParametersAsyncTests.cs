using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.CryptographicParametersResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetCryptographicParametersAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetCryptographicParametersAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_block_exists_should_return_correct_data()
    {
        // Arrange
        var emptyCryptographicParameters = new CryptographicParameters();
        var blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

        // Act
        var cryptographicParameters = await ConcordiumNodeClient.GetCryptographicParametersAsync(blockHash);

        // Assert
        cryptographicParameters.Should().NotBeNull();
        cryptographicParameters!.Value.Should().NotBeEquivalentTo(emptyCryptographicParameters);
    }
}
