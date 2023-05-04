using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Wallets;
using ConcordiumNetSdk.Examples;
using ConcordiumNetSdk.Client;

/// <summary>
/// Example demonstrating how to submit a register data transaction.
///
/// The example assumes you have your account key information stored
/// in the Concordium browser wallet key export format, and that a path
/// pointing to it is supplied to it from the command line.
///
/// Cfr. <see cref="RegisterDataTransactionExampleOptions"/> for more
/// info on how to run the program, or refer to the help message.
/// </summary>
class Program
{
    static void SendRegisterDataExample(RegisterDataTransactionExampleOptions options)
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

        // Create the register data transaction.
        // Encode a singleton string as CBOR and use that as the data to register.
        OnChainData data = OnChainData.FromTextEncodeAsCBOR(options.Data);
        RegisterData transferPayload = new RegisterData(data);

        // Prepare the transaction for signing.
        AccountAddress sender = account.AccountAddress;
        AccountSequenceNumber nonce = client.GetNextAccountSequenceNumber(sender);
        Expiry expiry = Expiry.AtMinutesFromNow(30);
        PreparedAccountTransaction<RegisterData> preparedTransfer = transferPayload.Prepare(
            sender,
            nonce,
            expiry
        );

        // Sign the transaction using the account keys.
        SignedAccountTransaction<RegisterData> signedTransfer = preparedTransfer.Sign(account);

        // Submit the transaction.
        TransactionHash txHash = client.SendTransaction(signedTransfer);
    }

    static void Main(string[] args)
    {
        Example.RunExample<RegisterDataTransactionExampleOptions>(args, SendRegisterDataExample);
    }
}
