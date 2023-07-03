using CommandLine;
using Concordium.Sdk.Wallets;

namespace Common;

/// <summary>
/// Command line options for the runnable SDK examples.
///
/// Like <see cref="ExampleOptions"/> with a further
/// option for specifying a path the path to a file
/// containing a supported import formats of
/// <see cref="WalletAccount"/>
/// at the command-line.
///
/// This is useful when writing examples that work with
/// account transactions.
/// </summary>
public abstract class ExampleOptionsWithKeys : ExampleOptions
{
    [Option(
        'k',
        "keys",
        HelpText = "Path to a file with contents that is in the Concordium browser wallet key export format.",
        Required = true
    )]
    public string WalletKeysFile { get; set; } = default!;
}
