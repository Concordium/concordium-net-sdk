using System.Diagnostics.CodeAnalysis;
using Concordium.Sdk.Types;
using FluentAssertions;
using Grpc.Core;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public sealed class GetAccountListAsync : Tests
{
    public GetAccountListAsync(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunGetAccountListAsync()
    {
        var block = BlockHash.From(this.GetString("blockHash"));

        var accountListAsync = this.Client.GetAccountListAsync(block);

        this.Output.WriteLine($"BlockHash: {block}");

        await foreach (var account in accountListAsync)
        {
            this.Output.WriteLine($"Account: {account}");
        }
    }

    [Fact]
    public async Task GivenBlockHashNotExist_WhenRunGetAccountListAsync_ThenNoneReturned()
    {
        var block = BlockHash.From("e0d3935527e313c3e5e6bd40afd062918a89215038273c27781bc2d71ca1de34");

        var accountListAsync = this.Client.GetAccountListAsync(block);

        this.Output.WriteLine($"BlockHash: {block}");

        Func<Task> action = async () => await accountListAsync.GetAsyncEnumerator().MoveNextAsync();
        await action.Should().ThrowAsync<RpcException>()
            .WithMessage("Status(StatusCode=\"NotFound\", Detail=\"block not found.\")");
    }
}
