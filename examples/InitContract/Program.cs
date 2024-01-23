using System.Globalization;
using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace InitContract;

internal sealed class InitContractOptions
{
    [Option(
        'k',
        "keys",
        HelpText = "Path to a file with contents that is in the Concordium browser wallet key export format.",
        Required = true
    )]
    public string WalletKeysFile { get; set; }
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
    [Option('a', "amount", HelpText = "Amount of CCD to deposit.", Default = 0)]
    public ulong Amount { get; set; }

    [Option('m', "module-ref", HelpText = "The module reference of the smart contract.", Required = true)]
    public string ModuleRef { get; set; }

    [Option('i', "init-name", HelpText = "The init_name of the module.", Required = true)]
    public string InitName { get; set; }

    [Option('e', "max-energy", HelpText = "The maximum energy to spend on the module.", Required = true)]
    public string MaxEnergy { get; set; }
}

public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBlockItems"/>
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<InitContractOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(InitContractOptions o)
    {
        // Read the account keys from a file.
        var walletData = File.ReadAllText(o.WalletKeysFile);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);

        // Construct the client.
        var clientOptions = new ConcordiumClientOptions
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        using var client = new ConcordiumClient(new Uri(o.Endpoint), clientOptions);

        // Create the transfer transaction.
        var amount = CcdAmount.FromCcd(o.Amount);
        var moduleRef = new ModuleReference(o.ModuleRef);
        var initName = new InitName(o.InitName);
        var param = new Parameter(Array.Empty<byte>());
        var maxEnergy = new EnergyAmount(uint.Parse(o.MaxEnergy, CultureInfo.InvariantCulture));
        var transferPayload = new Concordium.Sdk.Transactions.InitContract(amount, moduleRef, initName, param);

        // Prepare the transaction for signing.
        var sender = account.AccountAddress;
        var sequenceNumber = client.GetNextAccountSequenceNumber(sender).Item1;
        var expiry = Expiry.AtMinutesFromNow(30);
        var preparedTransfer = transferPayload.Prepare(sender, sequenceNumber, expiry, maxEnergy);

        // Sign the transaction using the account keys.
        var signedTransfer = preparedTransfer.Sign(account);

        // Submit the transaction.
        var txHash = client.SendAccountTransaction(signedTransfer);

        // Print the transaction hash.
        Console.WriteLine($"Successfully submitted transfer transaction with hash {txHash}");
    }
}

