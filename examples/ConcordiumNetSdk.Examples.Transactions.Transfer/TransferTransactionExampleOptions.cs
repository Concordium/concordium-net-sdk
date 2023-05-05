using CommandLine;

namespace ConcordiumNetSdk.Examples.Transactions;

public class TransferTransactionExampleOptions : TransactionExampleOptions
{
    public const UInt64 DEFAULT_AMOUNT = 100;

    [Option('a', "amount", HelpText = "Amount of CCD to transfer.", Default = DEFAULT_AMOUNT)]
    public UInt64 Amount { get; set; } = DEFAULT_AMOUNT;

    [Option('r', "receiver", HelpText = "Receiver of the CCD to transfer.", Required = true)]
    public string Receiver { get; set; } = "";
}
