﻿using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Wallets;
using ConcordiumNetSdk.Examples;
using ConcordiumNetSdk.Client;

/// <summary>
/// Example showing how to submit a transfer transaction.
///
/// The example assumes you have your account key information stored
/// in the format exported by the Concordium Browser Wallet, and a path
/// pointing to it is supplied to it from the command line.
/// </summary>
class Program
{
    static void SendTransferExample(TransferTransactionExampleOptions options)
    {
        // Read the account keys from a file.
        WalletAccount account = WalletAccount.FromBrowserWalletExportFormat(options.WalletKeysFile);

        // Construct the client.
        ConcordiumClient client = new ConcordiumClient(
            new Uri(options.Endpoint), // Endpoint URL.
            options.Port, // Port.
            60, // Use a timeout of 60 seconds.
            true // Use a secure connection.
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
    }

    static void Main(string[] args)
    {
        Example.RunExample<TransferTransactionExampleOptions>(args, SendTransferExample);
    }
}