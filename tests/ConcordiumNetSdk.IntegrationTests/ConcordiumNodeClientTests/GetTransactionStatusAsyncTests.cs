using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.IdentityProviderInfoResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetTransactionStatusAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetTransactionStatusAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_transaction_exists_should_return_correct_data()
    {
        // Arrange
        var emptyTransactionStatus = new IdentityProviderInfo();
        var transactionHash = TransactionHash.From("b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37");

        // Act
        var transactionStatus = await ConcordiumNodeClient.GetTransactionStatusAsync(transactionHash);

        // Assert
        transactionStatus.Should().NotBeNull();
        transactionStatus.Should().NotBeEquivalentTo(emptyTransactionStatus);
    }
}
