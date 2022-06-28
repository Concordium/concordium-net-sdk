using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.AccountInfoResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetAccountNonFinalizedTransactionsAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetAccountNonFinalizedTransactionsAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_account_exists_should_return_correct_data()
    {
        // Arrange
        var accountAddress = AccountAddress.From("32gxbDZj3aCr5RYnKJFkigPazHinKcnAhkxpade17htB4fj6DN");

        // Act
        var nonFinalizedTransactions = await ConcordiumNodeClient.GetAccountNonFinalizedTransactionsAsync(accountAddress);

        // Assert
        nonFinalizedTransactions.Should().NotBeNull();
    }
}
