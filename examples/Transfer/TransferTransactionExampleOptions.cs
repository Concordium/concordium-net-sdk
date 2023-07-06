using CommandLine;
using Common;

namespace Transactions.Transfer;

public sealed class TransferTransactionExampleOptions : ExampleOptionsWithKeys
{
    [Option('a', "amount", HelpText = "Amount of CCD to transfer.", Required = true)]
    public ulong Amount { get; set; } = 0;

    [Option('r', "receiver", HelpText = "Receiver of the CCD to transfer.", Required = true)]
    public string Receiver { get; set; } = default!;
}
