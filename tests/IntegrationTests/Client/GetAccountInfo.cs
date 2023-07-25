using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit.Abstractions;

namespace Concordium.Sdk.Tests.IntegrationTests.Client;

[Trait("Category", "IntegrationTests")]
public class GetAccountInfo : Tests
{
    public GetAccountInfo(ITestOutputHelper output) : base(output)
    {}

    [Fact]
    public async Task GivenBakerZero_AtGenesisBlock_WhenGetAccountInfo_ThenReturnBakerZeroId()
    {
        // Arrange
        var block = BlockHash.From("4221332d34e1694168c2a0c0b3fd0f273809612cb13d000d5c2e00e85f50f796");
        var accountAddress = AccountAddress.From("48XGRnvQoG92T1AwETvW5pnJ1aRSPMKsWtGdKhTqyiNZzMk3Qn");

        // Act
        var accountInfoAsync = await this.Client.GetAccountInfoAsync(accountAddress, new Given(block));

        // Assert
        accountInfoAsync.Response.AccountStakingInfo.Should().NotBeNull();
        accountInfoAsync.Response.AccountStakingInfo!.Should().BeOfType<AccountBaker>();
        var baker = accountInfoAsync.Response.AccountStakingInfo! as AccountBaker;
        baker!.BakerInfo.BakerId.Id.Index.Should().Be(0);
    }

}
