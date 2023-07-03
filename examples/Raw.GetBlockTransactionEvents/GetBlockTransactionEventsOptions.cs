using CommandLine;
using Common;

namespace RawClient.GetBlockTransactionEvents;

public sealed class GetBlockTransactionEventsExampleOptions : ExampleOptions
{
    private const string DefaultBlockHash = "lastfinal";

    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block from which to retrieve the account information from (can be \"lastfinal\", \"best\" or a block hash).",
        Required = true,
        Default = DefaultBlockHash
    )]
    public string BlockHash { get; set; } = default!;
}
