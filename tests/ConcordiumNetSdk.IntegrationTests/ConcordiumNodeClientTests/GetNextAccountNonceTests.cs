using System.Threading.Tasks;
using ConcordiumNetSdk.Responses.NextAccountNonceResponse;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.IntegrationTests.ConcordiumNodeClientTests;

public class GetNextAccountNonceTests
{
    private ConcordiumNodeClient ConcordiumNodeClient { get; }

    public GetNextAccountNonceTests()
    {
        var connection = new Connection {Address = "http://localhost:10001", AuthenticationToken = "rpcadmin"};
        ConcordiumNodeClient = new ConcordiumNodeClient(connection);
    }

    [Fact]
    public async Task When_account_exists_should_return_correct_data()
    {
        // Arrange
        // todo: think of better implementation of empty object
        var emptyNextAccountNonce = new NextAccountNonce();
        var accountAddress = new AccountAddress("32gxbDZj3aCr5RYnKJFkigPazHinKcnAhkxpade17htB4fj6DN");

        // Act
        var nextAccountNonce = await ConcordiumNodeClient.GetNextAccountNonceAsync(accountAddress);

        // Assert
        nextAccountNonce.Should().NotBeNull();
        nextAccountNonce.Should().NotBeEquivalentTo(emptyNextAccountNonce);
    }
}
