using CommandLine;

using Concordium.Sdk.Examples.Common;

namespace Concordium.Sdk.Examples.RawClient.GetAccountInfo;

public class GetAccountInfoExampleOptions : ExampleOptions
{
    private const string DefaultBlockHash = "lastfinal";

    [Option(
        'a',
        "account-address",
        HelpText = "Address of the account to retrieve the info of.",
        Required = true
    )]
    public string AccountAddress { get; set; } = default!;

    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block from which to retrieve the account information from (can be \"lastfinal\", \"best\" or a block hash).",
        Required = true,
        Default = DefaultBlockHash
    )]
    public string BlockHash { get; set; } = default!;
}
