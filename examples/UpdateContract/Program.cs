using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;

namespace Transactions.UpdateContract;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

internal sealed class UpdateTransactionExampleOptions
{
    [Option(
        'k',
        "keys",
        HelpText = "Path to a file with contents that is in the Concordium browser wallet key export format.",
        Required = true
    )]
    public string WalletKeysFile { get; set; }
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "https://grpc.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
    [Option('a', "amount", HelpText = "Amount of CCD to deposit.", Default = 0)]
    public ulong Amount { get; set; }

    [Option('c', "contract", HelpText = "The index of the smart contract.", Required = true)]
    public ulong Contract { get; set; }

    [Option('r', "receive-name", HelpText = "The receive_name of the contract to be called.", Required = true)]
    public string ReceiveName { get; set; }

    [Option('e', "max-energy", HelpText = "The maximum energy to spend on the module.", Required = true)]
    public ulong MaxEnergy { get; set; }
}

/// <summary>
/// Example demonstrating how to submit a transaction updating a smart contract.
///
/// The example assumes you have your account key information stored
/// in the Concordium browser wallet key export format, and that a path
/// pointing to it is supplied to it from the command line.
///
/// See <see cref="UpdateTransactionExampleOptions"/> for more info
/// on how to run the program, or refer to the help message.
/// </summary>
internal class Program
{
    /// <summary>
    /// Example send a contract update transaction.
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<UpdateTransactionExampleOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(UpdateTransactionExampleOptions options)
    {
        // Read the account keys from a file.
        var walletData = File.ReadAllText(options.WalletKeysFile);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);

        // Construct the client.
        var clientOptions = new ConcordiumClientOptions
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        using var client = new ConcordiumClient(new Uri(options.Endpoint), clientOptions);

        // Create the update transaction.
        var amount = CcdAmount.FromCcd(options.Amount);
        var contract = ContractAddress.From(options.Contract, 0);
        var receiveName = ReceiveName.Parse(options.ReceiveName);
        var parameter = Parameter.Empty();
        var maxEnergy = new EnergyAmount(options.MaxEnergy);

        var updatePayload = new Concordium.Sdk.Transactions.UpdateContract(amount, contract, receiveName, parameter);

        // Prepare the transaction for signing.
        var sender = account.AccountAddress;
        var sequenceNumber = client.GetNextAccountSequenceNumber(sender).Item1;
        var expiry = Expiry.AtMinutesFromNow(30);
        var preparedPayload = updatePayload.Prepare(sender, sequenceNumber, expiry, maxEnergy);

        // Sign the transaction using the account keys.
        var signedTransaction = preparedPayload.Sign(account);

        // Submit the transaction.
        var txHash = await client.SendAccountTransactionAsync(signedTransaction);

        // Print the transaction hash.
        Console.WriteLine($"Successfully submitted transfer transaction with hash {txHash}");
    }
}
