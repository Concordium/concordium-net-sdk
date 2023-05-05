using CommandLine;

namespace ConcordiumNetSdk.Examples.Transactions;

public class RegisterDataTransactionExampleOptions : TransactionExampleOptions
{
    [Option('d', "data", HelpText = "The data to register on-chain.", Required = true)]
    public string Data { get; set; } = "";
}
