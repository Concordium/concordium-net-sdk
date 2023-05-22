using Concordium.Grpc.V2;
using Concordium.Sdk.Client;
using Concordium.Sdk.Examples.Common;

namespace Concordium.Sdk.Examples.RawClient.GetBlockTransactionEvents;

/// <summary>
/// Example demonstrating the use of <see cref="Client.RawClient.GetBlockTransactionEvents"/>.
///
/// <see cref="RawClient"/> wraps methods of the Concordium Node gRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library.
/// </summary>
internal class Program
{
    private static async Task GetBlockTransactionEvents(
        GetBlockTransactionEventsExampleOptions options
    )
    {
        // Construct the client.
        using var client = new ConcordiumClient(
            new Uri(options.Endpoint),
            options.Port,
            options.Timeout
        );
        var blockHashInput = options.BlockHash.ToLowerInvariant() switch
        {
            "best" => new BlockHashInput() { Best = new Empty() },
            "lastfinal" => new BlockHashInput() { LastFinal = new Empty() },
            _ => Types.BlockHash.From(options.BlockHash).ToBlockHashInput(),
        };

        // Invoke the raw call.
        var events = client.Raw.GetBlockTransactionEvents(blockHashInput);

        // Print the stream elements as they arrive.
        await foreach (var e in events)
        {
            var details = e.DetailsCase switch
            {
                BlockItemSummary.DetailsOneofCase.AccountTransaction
                    => e.AccountTransaction.ToString(),
                BlockItemSummary.DetailsOneofCase.Update => e.Update.ToString(),
                BlockItemSummary.DetailsOneofCase.AccountCreation => e.AccountCreation.ToString(),
                BlockItemSummary.DetailsOneofCase.None => throw new NotImplementedException(),
                _ => "Block item summary did not contain any details",
            };
            var txHash = Types.TransactionHash.From(e.Hash.Value.ToByteArray());
            Console.WriteLine(
                $@"
                Got event with index {e.Index.Value} for transaction hash {txHash}:
                {details}
            "
            );
        }
    }

    private static async void Main(string[] args) =>
        await Example.RunAsync<GetBlockTransactionEventsExampleOptions>(args, GetBlockTransactionEvents);
}
