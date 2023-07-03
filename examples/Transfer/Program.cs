using Concordium.Sdk.Client;
using Concordium.Sdk.Examples.Common;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;

namespace Transactions.Transfer;

/// <summary>
/// Example demonstrating how to submit a transfer transaction.
///
/// The example assumes you have your account key information stored
/// in the Concordium browser wallet key export format, and that a path
/// pointing to it is supplied to it from the command line.
///
/// Cfr. <see cref="TransferTransactionExampleOptions"/> for more info
/// on how to run the program, or refer to the help message.
/// </summary>
internal class Program
{
    private static void SendTransferTransaction(TransferTransactionExampleOptions options)
    {
        // Read the account keys from a file.
        var walletData = File.ReadAllText(options.WalletKeysFile);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);

        // Construct the client.
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = new Uri($"{options!.Endpoint}:{options.Port}"),
            Timeout = TimeSpan.FromSeconds(options.Timeout)
        };
        using var client = new ConcordiumClient(clientOptions);

        // Create the transfer transaction.
        var amount = CcdAmount.FromCcd(options.Amount);
        var receiver = AccountAddress.From(options.Receiver);
        var transferPayload = new Concordium.Sdk.Transactions.Transfer(amount, receiver);

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

    private static void Main(string[] args) =>
        Example.Run<TransferTransactionExampleOptions>(args, SendTransferTransaction);
}
