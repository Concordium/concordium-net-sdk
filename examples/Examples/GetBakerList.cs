using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

public sealed class GetBakerList : Tests
{
    public GetBakerList(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetBakerListAsync()
    {
        var bakers = await this.Client.GetBakerListAsync(new LastFinal());

        this.Output.WriteLine($"BlockHash: {bakers.BlockHash}");
        await foreach (var baker in bakers.Response)
        {
            this.Output.WriteLine($"Id: {baker}");
        }
    }
}
