using CommandLine;

namespace Concordium.Sdk.Examples;

/// <summary>
/// Command line options for the runnable SDK examples
/// relating to account transactions.
///
/// This class can be used to specify the path to a file
/// containing a supported import formats of
/// <see cref="Concordium.Sdk.Wallets.WalletAccount"/>
/// at the command-line.
/// </summary>
public abstract class TransactionExampleOptions : ExampleOptions
{
    [Option(
        'k',
        "keys",
        HelpText = "Path to a file with contents that is in the Concordium browser wallet key export format.",
        Required = true
    )]
    public string WalletKeysFile { get; set; } = "";
}
