using CommandLine;

namespace ConcordiumNetSdk.Examples;

public class RegisterDataTransactionExampleOptions : TransactionExampleOptions
{
    public const string DEFAULT_DATA = "Hello, World!";

    [Option('d', "data", HelpText = "The data to register on-chain.", Required = true)]
    public string Data { get; set; } = DEFAULT_DATA;
}
