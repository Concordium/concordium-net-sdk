﻿using CommandLine;

namespace ConcordiumNetSdk.Examples.Raw;

public class GetAccountInfoExampleOptions : ExampleOptions
{
    const string DEFAULT_BLOCK_HASH = "lastfinal";

    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block from which to retrieve the account information from (can be \"lastfinal\", \"best\" or a block hash).",
        Required = true,
        Default = DEFAULT_BLOCK_HASH
    )]
    public string BlockHash { get; set; } = DEFAULT_BLOCK_HASH;
}