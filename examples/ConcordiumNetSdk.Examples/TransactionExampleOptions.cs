using CommandLine;

namespace ConcordiumNetSdk.Examples;

public abstract class TransactionExampleOptions : ExampleOptions
{
    [Option(
        's',
        "sender",
        HelpText = "Path to file containing keys in either the browser or genesis wallet key export format.",
        Required = true
    )]
    public string WalletKeysFile { get; set; } = "";
}
