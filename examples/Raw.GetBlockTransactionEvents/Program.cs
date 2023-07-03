using Common;
using Concordium.Grpc.V2;
using Concordium.Sdk.Client;
using Grpc.Core;

namespace RawClient.GetBlockTransactionEvents;

/// <summary>
/// Example demonstrating the use of <see cref="Concordium.Sdk.Client.RawClient.GetBlockTransactionEvents"/>.
///
/// <see cref="Concordium.Sdk.Client.RawClient"/> wraps methods of the Concordium Node gRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library.
/// </summary>
internal class Program
{
    private static async Task GetBlockTransactionEvents(
        GetBlockTransactionEventsExampleOptions options
    )
    {
        // Construct the client.
        var clientOptions = new ConcordiumClientOptions
        {
            Endpoint = new Uri($"{options!.Endpoint}:{options.Port}/"),
            Timeout = TimeSpan.FromSeconds(options.Timeout)
        };
        using var client = new ConcordiumClient(clientOptions);

        var blockHashInput = options.BlockHash.ToLowerInvariant() switch
        {
            "best" => new BlockHashInput() { Best = new Empty() },
            "lastfinal" => new BlockHashInput() { LastFinal = new Empty() },
            _ => Concordium.Sdk.Types.BlockHash.From(options.BlockHash).ToBlockHashInput(),
        };

        // Invoke the raw call.
        var events = client.Raw.GetBlockTransactionEvents(blockHashInput).ResponseStream.ReadAllAsync();

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
            var txHash = Concordium.Sdk.Types.TransactionHash.From(e.Hash.Value.ToByteArray());
            Console.WriteLine(
                $@"
                Got event with index {e.Index.Value} for transaction hash {txHash}:
                {details}
            "
            );
        }
    }

    private static async Task Main(string[] args) =>
        await Example.RunAsync<GetBlockTransactionEventsExampleOptions>(args, GetBlockTransactionEvents);
}
