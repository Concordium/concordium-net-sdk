using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;

namespace Transactions.UpdateContractMint;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

internal sealed class Options
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
    [Option("contract", HelpText = "The index of the smart contract.", Default = "7936")]
    public ulong Contract { get; set; }
    [Option("token", HelpText = "Token ID to mint", Default = "00")]
    public string TokenId { get; set; }
    [Option("metadata-url", HelpText = "URL to the metadata", Default = "")]
    public string MetadataUrl { get; set; }
    [Option("max-energy", HelpText = "The maximum energy to spend on the module.", Default = 5000)]
    public ulong MaxEnergy { get; set; }
}

/// <summary>
/// Mint CIS-2 tokens for the 'cis2_multi' smart contract example.
///
/// https://github.com/Concordium/concordium-rust-smart-contracts/blob/86511efac8e335abac66176df895c21a5cde252c/examples/cis2-multi/src/lib.rs
///
/// This example demonstrates:
///
/// - Submitting a transaction updating a smart contract, where the parameter is constructed
///   using a smart contract module schema.
/// - Waiting for the transaction to finalize.
/// - Checking the outcome and reading the logged events of the contract, parsing the events using
///   a smart contract module schema.
///
/// The example assumes:
/// - You have your account key information stored in the Concordium browser wallet key export
///   format, and that a path pointing to it is supplied to it from the command line.
/// - The 'cis2_multi' smart contract example is deployed on chain (already on testnet) with a
///   module reference matching the value of `CIS2_MULTI_MODULE_REF`.
/// - You have the contract address of an instance of the 'cis2_multi' smart contract (On testnet
///   the contract with index 7936 can be used).
/// </summary>
internal class Program
{
    /// <summary>
    /// Example send a contract update transaction.
    /// </summary>
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<Options>(args)
            .WithParsedAsync(Run);

    /// Module reference for a module containing the 'cis2_multi' smart contract example
    private static readonly string _cis2MultiModuleRef =
        "755d0e1f2820a3285e23ac4fa1862ae2dfa75ab8927133904e04fea7e9f1f4c9";

    /// Receive name for the 'mint' entrypoint of the contract.
    private static readonly string _cis2MultiReceiveMint = "cis2_multi.mint";

    private static async Task Run(Options options)
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
        var amount = CcdAmount.Zero;
        var contract = ContractAddress.From(options.Contract, 0);

        // Fetch the module source from chain, to extract the embedded schema.
        var moduleReference = new ModuleReference(_cis2MultiModuleRef);
        var moduleSourceResponse = await client.GetModuleSourceAsync(new LastFinal(), moduleReference);
        var moduleSchema = moduleSourceResponse.Response.GetModuleSchema()!;

        var receiveName = ReceiveName.Parse(_cis2MultiReceiveMint);

        // Construct the mint parameter, this parameter class is tied to details of the smart
        // contract itself.
        var parameter = new MintParameter
        {
            Owner = new JsonAccountAddr
            {
                Account = new[] { account.AccountAddress.ToString() }
            },
            MetadataUrl = new JsonMetadataUrl
            {
                Url = options.MetadataUrl,
                ContentHash = new JsonNoHash
                {
                    None = Array.Empty<int>()
                },
            },
            TokenId = options.TokenId
        };
        var jsonString = JsonSerializer.Serialize(parameter, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine($"Mint using the JSON parameter:\n{jsonString}");
        var jsonParameter = new Utf8Json(JsonSerializer.SerializeToUtf8Bytes(parameter));

        // Convert the JSON parameter into the byte format expected by the smart contract using the
        // information in the smart contract module schema.
        var parameterBytes = Parameter.UpdateJson(
            moduleSchema,
            receiveName.GetContractName(),
            receiveName.GetEntrypoint(),
            jsonParameter
        );


        // Prepare the transaction for signing.
        var updatePayload = new Concordium.Sdk.Transactions.UpdateContract(
            amount,
            contract,
            receiveName,
            parameterBytes
        );
        var sender = account.AccountAddress;
        var sequenceNumber = client.GetNextAccountSequenceNumber(sender).Item1;
        var expiry = Expiry.AtMinutesFromNow(30);
        var maxEnergy = new EnergyAmount(options.MaxEnergy);
        var preparedPayload = updatePayload.Prepare(sender, sequenceNumber, expiry, maxEnergy);

        // Sign the transaction using the account keys.
        var signedTransaction = preparedPayload.Sign(account);

        // Submit the transaction.
        var txHash = await client.SendAccountTransactionAsync(signedTransaction);
        Console.WriteLine($"Successfully submitted transfer transaction with hash {txHash}");

        // Watch the status of the transaction, until it becomes finalized in a block.
        Console.WriteLine($"Waiting for the transaction to finalize...");
        var finalized = await client.WaitUntilFinalized(txHash);

        Console.WriteLine($"Finalized in block with hash {finalized.BlockHash}");

        // Check whether the transaction was rejected and if so exit.
        if (finalized.Summary.TryGetRejectedAccountTransaction(out var reason))
        {
            Console.WriteLine($"Transaction got rejected:\n{reason}");
            return;
        }
        Console.WriteLine($"Transaction got accepted!");

        // Exact the logs of updated smart contract instances from the outcome.
        if (!finalized.Summary.TryGetContractUpdateLogs(out var updates))
        {
            throw new InvalidOperationException(
                "Transaction summary failed to parse as a contract update transaction."
            );
        }
        // Print out the events from each updated contract in the block.
        foreach (var update in updates)
        {
            var updatedContract = update.Item1;
            Console.WriteLine($"Contract {updatedContract} logged:");
            // If this is our contract, then we use the module schema to parse the events.
            if (updatedContract == contract)
            {
                foreach (var evt in update.Item2)
                {
                    // Use the smart contract module schema to deserialize the events.
                    var json = evt.GetDeserializeEvent(moduleSchema, receiveName.GetContractName());
                    Console.WriteLine($"- {json}");
                }
            }
            else
            {
                // For other contracts we just log the event bytes as a hex encoded string.
                foreach (var evt in update.Item2)
                {
                    Console.WriteLine($"- {evt.ToHexString()}");
                }
            }
        }
    }

    /// <summary>
    /// Class for constructing the JSON representation of the smart contract parameter for 'mint'
    /// entrypoint of 'cis2_multi' contract.
    /// The exact structure will depend on the details of the smart contract module.
    /// </summary>
    private class MintParameter
    {
        /// <summary>
        /// The owner for the newly minted tokens.
        ///
        /// This property only represents the owner being an account address, but the smart contract
        /// also allow for a contract address here, which we choose not include in the type for
        /// simplicity.
        /// </summary>
        [JsonPropertyName("owner")]
        public JsonAccountAddr Owner { get; set; }
        /// <summary>
        /// The metadata url for a newly minted token type.
        /// </summary>
        [JsonPropertyName("metadata_url")]
        public JsonMetadataUrl MetadataUrl { get; set; }
        /// <summary>
        /// The TokenID identifying the token type to mint.
        /// </summary>
        [JsonPropertyName("token_id")]
        public string TokenId { get; set; }
    }

    /// <summary>
    /// Indicator class for some address being an account address.
    /// Note this only represents a single account address, but still contains an array of strings
    /// for the JSON format to match.
    /// </summary>
    private class JsonAccountAddr
    {
        public string[] Account { get; set; }
    }

    /// <summary>
    /// Class for constructing the JSON representation of the token metadata URL.
    /// </summary>
    private class JsonMetadataUrl
    {
        /// <summary>
        /// The URL pointing to the token metadata.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
        /// <summary>
        /// The hash of the token metadata.
        ///
        /// This property only supports not providing a content hash of the metadata, but the smart
        /// contract supports this as well, we choose not include this in the type for simplicity.
        /// </summary>
        [JsonPropertyName("hash")]
        public JsonNoHash ContentHash { get; set; }
    }

    /// <summary>
    /// Indicator of no checksum (SHA256 hash) provided for the token metadata.
    /// Note this still contains an array of int for the JSON format to match.
    /// </summary>
    private class JsonNoHash
    {
        public int[] None { get; set; }
    }
}
