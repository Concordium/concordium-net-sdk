using CommandLine;

namespace Concordium.Sdk.Examples.Transactions;

public class TransferTransactionExampleOptions : TransactionExampleOptions
{
    [Option('a', "amount", HelpText = "Amount of CCD to transfer.", Required = true)]
    public UInt64 Amount { get; set; } = 0;

    [Option('r', "receiver", HelpText = "Receiver of the CCD to transfer.", Required = true)]
    public string Receiver { get; set; } = "";
}
