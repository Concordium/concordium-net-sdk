using Concordium.Sdk.Tests.IntegrationTests.Utils.LocalNode;
using Concordium.Sdk.Types;
using FluentAssertions;

namespace Concordium.Sdk.Tests.IntegrationTests.Types;

[Trait("Category", "IntegrationLocalNodeTests")]
[Collection(LocalNodeCollectionFixture.LocalNodes)]
public class BlockInfoTests
{
    private readonly LocalNodeFixture _fixture;

    public BlockInfoTests(LocalNodeFixture fixture) => this._fixture = fixture;

    [Fact(Timeout = LocalNodeFixture.Timeout)]
    public async Task GivenProtocolEqualOrAbove6_WhenGetConsensusInfo_ThenParse()
    {
        // Arrange
        using var cts = new CancellationTokenSource(LocalNodeFixture.Timeout);

        // Act
        var blockInfo = await this._fixture.Client.GetBlockInfoAsync(new LastFinal(), cts.Token);

        // Assert
        blockInfo.Response.BlockSlot.Should().BeNull();
        blockInfo.Response.Round.Should().NotBeNull();
        blockInfo.Response.Epoch.Should().NotBeNull();
    }
}
