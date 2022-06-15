using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.TransactionStatusInBlockResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetTransactionStatusInBlockAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetTransactionStatusInBlockAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_transaction_and_block_exists_should_return_correct_data()
    {
        // Arrange
        var emptyTransactionStatusInBlock = new TransactionStatusInBlock();
        var transactionHash = TransactionHash.From("b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37");
        var blockHash = BlockHash.From("1895d3dfd287afc125927f46ae395718dcebcd9b98562903b0030968428f5179");

        // Act
        var transactionStatus = await ConcordiumNodeClient.GetTransactionStatusInBlockAsync(transactionHash, blockHash);

        // Assert
        transactionStatus.Should().NotBeNull();
        transactionStatus.Should().NotBeEquivalentTo(emptyTransactionStatusInBlock);
    }
}
