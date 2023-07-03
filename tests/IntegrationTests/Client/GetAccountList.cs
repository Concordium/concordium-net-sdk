using System.Diagnostics.CodeAnalysis;
using Concordium.Sdk.Types;
using FluentAssertions;
using Grpc.Core;
using Xunit.Abstractions;
using Xunit;

namespace Concordium.Sdk.Tests.IntegrationTests.Client;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
[Trait( "Category", "IntegrationTests")]
public sealed class GetAccountList : Tests
{
    public GetAccountList(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenBlockHashNotExist_WhenRunGetAccountListAsync_ThenNoneReturned()
    {
        var block = BlockHash.From("e0d3935527e313c3e5e6bd40afd062918a89215038273c27781bc2d71ca1de34");

        var response = await this.Client.GetAccountListAsync(new Given(block));

        this.Output.WriteLine($"BlockHash: {response.BlockHash}");

        Func<Task> action = async () => await response.Response.GetAsyncEnumerator().MoveNextAsync();
        await action.Should().ThrowAsync<RpcException>()
            .WithMessage("Status(StatusCode=\"NotFound\", Detail=\"block not found.\")");
    }
}
