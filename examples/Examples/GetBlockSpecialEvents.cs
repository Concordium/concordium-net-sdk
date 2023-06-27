using Concordium.Sdk.Types;
using Xunit.Abstractions;
using BlockHash = Concordium.Sdk.Types.BlockHash;

namespace Concordium.Sdk.Examples;

public sealed class GetBlockSpecialEvents : Tests
{
    public GetBlockSpecialEvents(ITestOutputHelper output) : base(output) {}

    [Fact]
    public async Task RunGetBlockSpecialEvents()
    {
        var block = BlockHash.From(this.GetString("blockHashWithEvents"));
        await foreach(var specialEvent in this.Client.GetBlockSpecialEvents(new Given(block)))
        {
            this.Output.WriteLine($"Type of special event is: {specialEvent.GetType().Name}");
        }
    }
}
