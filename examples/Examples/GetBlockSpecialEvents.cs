using Concordium.Sdk.Types;
using Xunit.Abstractions;
using BlockHash = Concordium.Sdk.Types.BlockHash;

namespace Concordium.Sdk.Examples;

public sealed class GetBlockSpecialEvents : Tests
{
    public GetBlockSpecialEvents(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task RunGetBlockSpecialEvents()
    {
        var block = BlockHash.From(this.GetString("blockHashWithEvents"));
        var response = await this.Client.GetBlockSpecialEvents(new Given(block));

        this.Output.WriteLine($"BlockHash: {response.BlockHash}");
        await foreach (var specialEvent in response.Response)
        {
            this.Output.WriteLine($"Type of special event is: {specialEvent.GetType().Name}");
        }
    }
}
