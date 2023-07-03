using Concordium.Sdk.Types;
using FluentAssertions;
using Grpc.Core;
using Xunit.Abstractions;

namespace Concordium.Sdk.Tests.IntegrationTests.Client;

[Trait("Category", "IntegrationTests")]
public sealed class GetAccountList : Tests
{
    public GetAccountList(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenBlockHashNotExist_WhenRunGetAccountListAsync_ThenThrowExceptionNoneReturned()
    {
        // Arrange
        var block = BlockHash.From("e0d3935527e313c3e5e6bd40afd062918a89215038273c27781bc2d71ca1de34");
        var response = () => this.Client.GetAccountListAsync(new Given(block));

        // Act & Assert
        await response.Should().ThrowAsync<RpcException>()
            .WithMessage("Status(StatusCode=\"NotFound\", Detail=\"block not found.\")");
    }
}
