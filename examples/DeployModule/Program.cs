using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace DeployModule;

internal sealed class DeployModuleOptions
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

    [Option('m', "module-file", HelpText = "Path to the smart contract module.", Required = true)]
    public string ModulePath { get; set; }
}

public static class Program
{
    /// <summary>
    /// Example demonstrating how to submit a deploy module transaction.
    ///
    /// The example assumes you have your account key information stored
    /// in the Concordium browser wallet key export format, and that a path
    /// pointing to it is supplied to it from the command line.
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<DeployModuleOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(DeployModuleOptions o)
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
        var b = File.ReadAllBytes(o.ModulePath);
        var versionedModule = VersionedModuleSourceFactory.FromFile(o.ModulePath);
        var transferPayload = new Concordium.Sdk.Transactions.DeployModule(versionedModule);

        // Prepare the transaction for signing.
        var sender = account.AccountAddress;
        var sequenceNumber = client.GetNextAccountSequenceNumber(sender).Item1;
        var expiry = Expiry.AtMinutesFromNow(30);
        var preparedTransfer = transferPayload.Prepare(sender, sequenceNumber, expiry);

        // Sign the transaction using the account keys.
        var signedTransfer = preparedTransfer.Sign(account);

        // Submit the transaction.
        var txHash = client.SendAccountTransaction(signedTransfer);

        // Print the transaction hash.
        Console.WriteLine($"Successfully submitted transfer transaction with hash {txHash}");
    }
}

