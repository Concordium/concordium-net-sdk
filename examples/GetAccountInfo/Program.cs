using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

namespace GetAccountInfo;

internal sealed class GetAccountInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.", Required = true,
        Default = "http://node.testnet.concordium.com:20000/")]
    public Uri Uri { get; set; }

    [Option(
        'a',
        "account-address",
        HelpText = "Address of the account to retrieve the info of.",
        Required = true
    )]
    public string AccountAddress { get; set; }

    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block.",
        Required = true
    )]
    public string BlockHash { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetAccountInfoAsync"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetAccountInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetAccountInfoOptions options)
    {
        var accountAddress = AccountAddress.From(options.AccountAddress);
        var block = BlockHash.From(options.BlockHash);

        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = options.Uri
        };
        using var client = new ConcordiumClient(clientOptions);

        var response = await client.GetAccountInfoAsync(accountAddress, new Given(block));
        var accountInfo = response.Response;

        Console.WriteLine($"Blockhash: {response.BlockHash}");

        if (accountInfo.AccountStakingInfo is null)
        {
            Console.WriteLine($"Address: {accountInfo.AccountAddress} doesn't stake");
            return;
        }

        switch (accountInfo.AccountStakingInfo)
        {
            case AccountBaker accountBaker:
                Console.WriteLine($"Account is baker with staked CCD amount: {accountBaker.StakedAmount.GetFormattedCcd()}.");
                break;
            case AccountDelegation accountDelegation:
                Console.WriteLine($"Account is delegating CCD amount: {accountDelegation.StakedAmount.GetFormattedCcd()}.");

                if (accountDelegation.PendingChange is null)
                {
                    break;
                }

                switch (accountDelegation.PendingChange)
                {
                    case ReduceStakePending reduce:
                        Console.WriteLine($"At {reduce.EffectiveTime} new stake wil be {reduce.NewStake.GetFormattedCcd()}.");
                        break;
                    case RemoveStakePending remove:
                        Console.WriteLine($"At {remove.EffectiveTime} stake will be removed.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(new($"unknown type: {accountDelegation.PendingChange!.GetType()}"));
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(new($"unknown type: {accountInfo.AccountStakingInfo!.GetType()}"));
        }
    }
}
