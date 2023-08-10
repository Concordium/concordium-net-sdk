using Concordium.Sdk.Types;
using FluentAssertions;
using Grpc.Core;
using Xunit.Abstractions;

namespace Concordium.Sdk.Tests.IntegrationTests.Client;

[Trait("Category", "IntegrationTests")]
public sealed class GetBlockInfo : Tests
{
    public GetBlockInfo(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenBlockWithOneTransaction_WhenGetBlockInfo_ThenTransactionCountOne()
    {
        // Arrange
        var blochHashString = this.GetString("blockHashOneTransaction");


        // Act
        var info = await this.Client.GetBlockInfoAsync(new Given(BlockHash.From(blochHashString)));

        // Assert
        info.Response.TransactionCount.Should().Be(1);
    }

    [Fact]
    public async Task GivenAbsoluteHeightAboveBlockHeight_WhenCallGetBlockInfo_ThenThrowException()
    {
        // Arrange
        var blockInput = new Absolute(ulong.MaxValue);
        Func<Task> action = async () => await this.Client.GetBlockInfoAsync(blockInput, CancellationToken.None);

        // Act & Assert
        await action.Should().ThrowAsync<RpcException>();
    }

}
