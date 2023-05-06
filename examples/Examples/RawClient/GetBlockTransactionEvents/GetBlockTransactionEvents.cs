using Concordium.Sdk.Client;
using Concordium.Grpc.V2;

namespace Concordium.Sdk.Examples.RawClient;

/// <summary>
/// Example demonstrating the use of <see cref="RawClient.GetBlockTransactionEvents"/>.
///
/// <see cref="RawClient"/> wraps methods of the Concordium Node GRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library.
/// </summary>
class GetBlockTransactionEventsExample
{
    async static void GetBlockTransactionEvents(GetBlockTransactionEventsOptions options)
    {
        // Construct the client.
        ConcordiumClient client = new ConcordiumClient(
            new Uri(options.Endpoint), // Endpoint URL.
            options.Port, // Port.
            60 // Use a timeout of 60 seconds.
        );

        BlockHashInput blockHashInput;

        switch (options.BlockHash.ToLower())
        {
            case "best":
                blockHashInput = new BlockHashInput() { Best = new Empty() };
                break;
            case "lastfinal":
                blockHashInput = new BlockHashInput() { LastFinal = new Empty() };
                break;
            default:
                blockHashInput = Concordium.Sdk.Types.BlockHash
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

            var txHash = Concordium.Sdk.Types.TransactionHash.From(e.Hash.Value.ToByteArray());
            Console.WriteLine(
                $@"
                Got event with index {e.Index.Value.ToString()} for transaction hash {txHash.ToString()}:
                {details}
            "
            );
        }
    }

    static void Main(string[] args)
    {
        Example.Run<GetBlockTransactionEventsOptions>(args, GetBlockTransactionEvents);
    }
}
