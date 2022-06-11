using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.BirkParametersResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetModuleSourceAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetModuleSourceAsyncTests()
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
        var moduleRef = ModuleRef.From("7f60dc4d93e491750ed09d2abb379286c5af6f4aca2310c0b09c3275e181f4a4");

        // Act
        var byteString = await ConcordiumNodeClient.GetModuleSourceAsync(blockHash, moduleRef);

        // Assert
        byteString.Should().NotBeEmpty();
        byteString.Memory.ToArray().Should().NotBeEmpty();
    }
}
