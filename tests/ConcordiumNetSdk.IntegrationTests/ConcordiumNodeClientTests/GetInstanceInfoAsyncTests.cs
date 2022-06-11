using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.ContractInfoResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetInstanceInfoAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetInstanceInfoAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_account_and_block_exists_should_return_correct_data()
    {
        // Arrange
        // todo: think of better implementation of empty object
        var emptyContractInfo = new ContractInfo();
        var contractAddress = ContractAddress.Create(0, 0);
        var blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

        // Act
        var contractInfo = await ConcordiumNodeClient.GetInstanceInfoAsync(contractAddress, blockHash);

        // Assert
        contractInfo.Should().NotBeNull();
        contractInfo.Should().NotBeEquivalentTo(emptyContractInfo);
    }
}
