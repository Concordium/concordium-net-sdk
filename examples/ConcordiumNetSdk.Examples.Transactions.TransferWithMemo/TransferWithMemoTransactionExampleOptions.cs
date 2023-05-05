using CommandLine;

namespace ConcordiumNetSdk.Examples;

public class TransferWithMemoTransactionExampleOptions : TransactionExampleOptions
{
    public const UInt64 DEFAULT_AMOUNT = 100;

    [Option('a', "amount", HelpText = "Amount of CCD to transfer.", Default = 100)]
    public UInt64 Amount { get; set; } = DEFAULT_AMOUNT;

    [Option('r', "receiver", HelpText = "Receiver of the CCD to transfer.", Required = true)]
    public string Receiver { get; set; } = "";

    [Option('m', "memo", HelpText = "The memo to include with the transfer.", Required = true)]
    public string Memo { get; set; } = "";
}
