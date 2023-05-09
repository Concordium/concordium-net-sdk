using Concordium.Sdk.Types;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Wallets;
using Concordium.Sdk.Client;
using Concordium.Sdk.Examples.Common;

namespace Concordium.Sdk.Examples.Transactions;

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
    static void SendRegisterDataTransaction(RegisterDataTransactionExampleOptions options)
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

        // Encode a string as CBOR and use that as the data to register.
        OnChainData data = OnChainData.FromTextEncodeAsCBOR(options.Data);
        RegisterData transferPayload = new RegisterData(data);

        // Prepare the transaction for signing.
        AccountAddress sender = account.AccountAddress;
        AccountSequenceNumber nonce = client.GetNextAccountSequenceNumber(sender).Item1;
        Expiry expiry = Expiry.AtMinutesFromNow(30);
        PreparedAccountTransaction<RegisterData> preparedTransfer = transferPayload.Prepare(
            sender,
            nonce,
            expiry
        );

        // Sign the transaction using the account keys.
        SignedAccountTransaction<RegisterData> signedTransfer = preparedTransfer.Sign(account);

        // Try to submit the transaction.
        TransactionHash txHash = client.SendTransaction(signedTransfer);

        // Print the transaction hash.
        Console.WriteLine(
            $"Successfully submitted register data transaction with hash {txHash.ToString()}"
        );
    }

    static void Main(string[] args)
    {
        Example.Run<RegisterDataTransactionExampleOptions>(args, SendRegisterDataTransaction);
    }
}
