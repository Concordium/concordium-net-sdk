using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Wallets;
using ConcordiumNetSdk.Examples;
using ConcordiumNetSdk.Client;

namespace ConcordiumNetSdk.Examples.Transactions;

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
class Program
{
    static void SendTransferExample(TransferTransactionExampleOptions options)
    {
        // Read the account keys from a file.
        string walletData = File.ReadAllText(options.WalletKeysFile);
        WalletAccount account = WalletAccount.FromWalletKeyExportFormat(walletData);

        // Construct the client.
        ConcordiumClient client = new ConcordiumClient(
            new Uri(options.Endpoint), // Endpoint URL.
            options.Port, // Port.
            60 // Use a timeout of 60 seconds.
        );

        // Create the transfer transaction.
        CcdAmount amount = CcdAmount.FromCcd(options.Amount);
        AccountAddress receiver = AccountAddress.From(options.Receiver);
        Transfer transferPayload = new Transfer(amount, receiver);

        // Prepare the transaction for signing.
        AccountAddress sender = account.AccountAddress;
        AccountSequenceNumber nonce = client.GetNextAccountSequenceNumber(sender);
        Expiry expiry = Expiry.AtMinutesFromNow(30);
        PreparedAccountTransaction<Transfer> preparedTransfer = transferPayload.Prepare(
            sender,
            nonce,
            expiry
        );

        // Sign the transaction using the account keys.
        SignedAccountTransaction<Transfer> signedTransfer = preparedTransfer.Sign(account);

        // Submit the transaction.
        TransactionHash txHash = client.SendTransaction(signedTransfer);

        // Print the transaction hash.
        Console.WriteLine(
            $"Successfully submitted transfer transaction with hash {txHash.ToString()}"
        );
    }

    static void Main(string[] args)
    {
        Example.RunExample<TransferTransactionExampleOptions>(args, SendTransferExample);
    }
}
