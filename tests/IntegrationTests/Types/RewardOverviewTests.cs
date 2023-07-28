using Concordium.Sdk.Tests.IntegrationTests.Utils.LocalNode;
using Concordium.Sdk.Types;
using FluentAssertions;

namespace Concordium.Sdk.Tests.IntegrationTests.Types;

[Trait("Category", "IntegrationLocalNodeTests")]
[Collection(LocalNodeCollectionFixture.LocalNodes)]
public class RewardOverviewTests
{
    private readonly LocalNodeFixture _fixture;

    public RewardOverviewTests(LocalNodeFixture fixture) => this._fixture = fixture;

    [Fact(Timeout = LocalNodeFixture.Timeout)]
    public async Task GivenProtocolEqualOrAbove6_WhenGetConsensusInfo_ThenParse()
    {
        // Arrange
        using var cts = new CancellationTokenSource(LocalNodeFixture.Timeout);

        // Act
        var rewardOverview = await this._fixture.Client.GetTokenomicsInfoAsync(new LastFinal(), cts.Token);

        // Assert
        rewardOverview.Response.Should().NotBeNull();
    }
}
