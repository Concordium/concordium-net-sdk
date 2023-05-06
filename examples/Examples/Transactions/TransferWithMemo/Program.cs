using Concordium.Sdk.Types;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Wallets;
using Concordium.Sdk.Client;

namespace Concordium.Sdk.Examples.Transactions;

/// <summary>
/// Example demonstrating how to submit a transfer with memo transaction.
///
/// The example assumes you have your account key information stored
/// in the Concordium browser wallet key export format, and that a path
/// pointing to it is supplied to it from the command line.
///
/// Cfr. <see cref="TransferWithMemoTransactionExampleOptions"/> for more
/// info on how to run the program, or refer to the help message.
/// </summary>
class Program
{
    static void SendTransferWithMemoTransaction(TransferWithMemoTransactionExampleOptions options)
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
        // Encode a singleton string as CBOR and use that for the memo data.
        OnChainData memo = OnChainData.FromTextEncodeAsCBOR(options.Memo);
        TransferWithMemo transferPayload = new TransferWithMemo(amount, receiver, memo);

        // Prepare the transaction for signing.
        AccountAddress sender = account.AccountAddress;
        AccountSequenceNumber nonce = client.GetNextAccountSequenceNumber(sender);
        Expiry expiry = Expiry.AtMinutesFromNow(30);
        PreparedAccountTransaction<TransferWithMemo> preparedTransfer = transferPayload.Prepare(
            sender,
            nonce,
            expiry
        );

        // Sign the transaction using the account keys.
        SignedAccountTransaction<TransferWithMemo> signedTransfer = preparedTransfer.Sign(account);

        // Submit the transaction.
        TransactionHash txHash = client.SendTransaction(signedTransfer);

        // Print the transaction hash.
        Console.WriteLine(
            $"Successfully submitted transfer with memo transaction with hash {txHash.ToString()}"
        );
    }

    static void Main(string[] args)
    {
        Example.Run<TransferWithMemoTransactionExampleOptions>(
            args,
            SendTransferWithMemoTransaction
        );
    }
}
