using ConcordiumNetSdk.Client;
using Concordium.V2;

namespace ConcordiumNetSdk.Examples.Raw;

/// <summary>
/// Example demonstrating the use of <see cref="ConcordiumClient.RawClient.GetBlockTransactionEvents"/>.
///
/// <see cref="ConcordiumClient.RawClient"/> wraps methods of the Concordium Node GRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library.
/// </summary>
class Program
{
    async static void GetAccountInfoExample(GetAccountInfoExampleOptions options)
    {
        // Construct the client.
        ConcordiumClient client = new ConcordiumClient(
            new Uri(options.Endpoint), // Endpoint URL.
            options.Port, // Port.
            60, // Use a timeout of 60 seconds.
            false // Use a secure connection.
        );

        BlockHashInput blockHashInput;

        switch (options.BlockHash.ToLower())
        {
            case "best":
                blockHashInput = ConcordiumNetSdk.Types.BlockHash.BestBlockHashInput();
                break;
            case "lastfinal":
                blockHashInput = ConcordiumNetSdk.Types.BlockHash.LastFinalBlockHashInput();
                break;
            default:
                blockHashInput = ConcordiumNetSdk.Types.BlockHash
                    .From(options.BlockHash)
                    .ToBlockHashInput();
                break;
        }

        // Invoke the "raw" call.
        IAsyncEnumerable<BlockItemSummary> events = client.Raw.GetBlockTransactionEvents(
            blockHashInput
        );

        // Print the stream elements as they arrive.
        await foreach (BlockItemSummary e in events)
        {
            string details;
            switch (e.DetailsCase)
            {
                case BlockItemSummary.DetailsOneofCase.AccountTransaction:
                    details = e.AccountTransaction.ToString();
                    break;
                case BlockItemSummary.DetailsOneofCase.Update:
                    details = e.Update.ToString();
                    break;
                case BlockItemSummary.DetailsOneofCase.AccountCreation:
                    details = e.AccountCreation.ToString();
                    break;
                default:
                    details = "Block item summary did not contain any details";
                    break;
            }

            var txHash = ConcordiumNetSdk.Types.TransactionHash.From(e.Hash.Value.ToByteArray());
            Console.WriteLine(
                $@"
                Received event with index {e.Index.Value.ToString()} for transaction with hash {txHash.ToString()}:
                {details}
            "
            );
        }
    }

    static void Main(string[] args)
    {
        Example.RunExample<GetAccountInfoExampleOptions>(args, GetAccountInfoExample);
    }
}
