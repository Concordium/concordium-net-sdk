using CommandLine;

namespace ConcordiumNetSdk.Examples.Raw;

public class GetAccountInfoExampleOptions : ExampleOptions
{
    [Option(
        'b',
        "block-hash",
        HelpText = "Block hash of the block from which to retrieve the account information from.",
        Required = true
    )]
    public string BlockHash { get; set; } = "";
}
