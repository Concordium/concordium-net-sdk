using CommandLine;

namespace Concordium.Sdk.Examples.Transactions;

public class RegisterDataTransactionExampleOptions : ExampleOptionsWithKeys
{
    [Option('d', "data", HelpText = "The data to register on-chain.", Required = true)]
    public string Data { get; set; } = "";
}
