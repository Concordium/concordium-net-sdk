using Concordium.Grpc.V2;
using Concordium.Sdk.Client;
using Concordium.Sdk.Examples.Common;

namespace Concordium.Sdk.Examples.RawClient.GetFinalizedBlocks;

/// <summary>
/// Example demonstrating the use of <see cref="Client.RawClient.GetFinalizedBlocks"/>.
///
/// <see cref="RawClient"/> wraps methods of the Concordium Node gRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library. Creating an instance
/// of the generated <see cref="AccountInfoRequest"/> class used for the method input is given below.
/// </summary>
internal class Program
{
    private static async Task GetFinalizedBlocks(ExampleOptions options)
    {
        // Construct the client.
        var client = new ConcordiumClient(
            new Uri(options.Endpoint),
            options.Port,
            options.Timeout
        );

        // Invoke the raw call.
        var blocks = client.Raw.GetFinalizedBlocks();

        Console.WriteLine("Listening for finalized blocks:");
        await foreach (var blockInfo in blocks)
        {
            var blockHash = client.Raw.GetBlockInfo(
                new BlockHashInput() { Given = new BlockHash() { Value = blockInfo.Hash.Value } }
            );
            Console.WriteLine("Got a finalized block:");
            Console.WriteLine(blockHash.ToString());
        }
    }

    private static Task Main(string[] args) =>
        Example.Run<ExampleOptions>(args, GetFinalizedBlocks);
}
