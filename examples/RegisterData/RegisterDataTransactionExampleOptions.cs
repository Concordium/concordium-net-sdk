using CommandLine;
using Common;

namespace Transactions.RegisterData;

public class RegisterDataTransactionExampleOptions : ExampleOptionsWithKeys
{
    [Option('d', "data", HelpText = "The data to register on-chain.", Required = true)]
    public string Data { get; set; } = default!;
}
