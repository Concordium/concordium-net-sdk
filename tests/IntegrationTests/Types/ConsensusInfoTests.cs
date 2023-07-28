using Concordium.Sdk.Tests.IntegrationTests.Utils.LocalNode;
using FluentAssertions;

namespace Concordium.Sdk.Tests.IntegrationTests.Types;

[Trait("Category", "IntegrationLocalNodeTests")]
[Collection(LocalNodeCollectionFixture.LocalNodes)]
public class ConsensusInfoTests
{
    private readonly LocalNodeFixture _fixture;

    public ConsensusInfoTests(LocalNodeFixture fixture) => this._fixture = fixture;

    [Fact(Timeout = LocalNodeFixture.Timeout)]
    public async Task GivenProtocolEqualOrAbove6_WhenGetConsensusInfo_ThenParse()
    {
        // Arrange
        using var cts = new CancellationTokenSource(LocalNodeFixture.Timeout);

        // Act
        var consensusInfo = await this._fixture.Client.GetConsensusInfoAsync(cts.Token);

        // Assert
        consensusInfo.SlotDuration.Should().BeNull();
        consensusInfo.ConcordiumBftDetails.Should().NotBeNull();
    }
}
