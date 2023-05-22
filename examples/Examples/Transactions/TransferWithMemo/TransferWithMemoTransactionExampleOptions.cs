using CommandLine;

using Concordium.Sdk.Examples.Common;

namespace Concordium.Sdk.Examples.Transactions.TransferWithMemo;

public class TransferWithMemoTransactionExampleOptions : ExampleOptionsWithKeys
{
    [Option('a', "amount", HelpText = "Amount of CCD to transfer.", Required = true)]
    public ulong Amount { get; set; } = default!;

    [Option('r', "receiver", HelpText = "Receiver of the CCD to transfer.", Required = true)]
    public string Receiver { get; set; } = default!;

    [Option('m', "memo", HelpText = "The memo to include with the transfer.", Required = true)]
    public string Memo { get; set; } = default!;
}
