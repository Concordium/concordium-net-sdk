using ConcordiumNetSdk.Client;
using Concordium.V2;

namespace ConcordiumNetSdk.Examples.Raw;

/// <summary>
/// Example demonstrating the use of <see cref="RawClient.GetFinalizedBlocks"/>.
///
/// <see cref="RawClient"/> wraps methods of the Concordium Node GRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library. Creating an instance
/// of the generated <see cref="AccountInfoRequest"/> class used for the method input is given below.
/// </summary>
class Program
{
    static async Task GetFinalizedBlocksExample(ExampleOptions options)
    {
        // Construct the client.
        ConcordiumClient client = new ConcordiumClient(
            new Uri(options.Endpoint), // Endpoint URL.
            options.Port, // Port.
            60 // Use a timeout of 60 seconds.
        );

        // Invoke the "raw" call.
        IAsyncEnumerable<FinalizedBlockInfo> blocks = client.Raw.GetFinalizedBlocks();

        Console.WriteLine("Listening for finalized blocks:");
        await foreach (var blockInfo in blocks)
        {
            var blockHash = client.Raw.GetBlockInfo(
                new BlockHashInput()
                {
                    Given = new Concordium.V2.BlockHash() { Value = blockInfo.Hash.Value }
                }
            );
            Console.WriteLine("Got a finalized block:");
            Console.WriteLine(blockHash.ToString());
        }
    }

    static void Main(string[] args)
    {
        Example.Run<ExampleOptions>(args, GetFinalizedBlocksExample);
    }
}
