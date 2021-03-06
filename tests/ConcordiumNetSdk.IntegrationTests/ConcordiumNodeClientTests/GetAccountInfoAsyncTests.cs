using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.AccountInfoResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetAccountInfoAsyncTests
{
    private IConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetAccountInfoAsyncTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_account_exists_should_return_correct_data()
    {
        // Arrange
        // todo: think of better implementation of empty object
        var accountAddress = AccountAddress.From("32gxbDZj3aCr5RYnKJFkigPazHinKcnAhkxpade17htB4fj6DN");
        var blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

        // Act
        var accountInfo = await ConcordiumNodeClient.GetAccountInfoAsync(accountAddress, blockHash);

        // Assert
        accountInfo.Should().NotBeNull();
    }
}
