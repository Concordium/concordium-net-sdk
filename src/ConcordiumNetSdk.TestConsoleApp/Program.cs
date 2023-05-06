using Newtonsoft.Json;
using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Client;
using Concordium.V2;
using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;

// Create the client.
Uri url = new Uri("http://service.internal.testnet.concordium.com/");
UInt16 port = 20000;
ConcordiumClient client = new ConcordiumClient(url, port, 1000);

async Task Callback(int a)
{
    var blocks = client.Raw.GetFinalizedBlocks();
    Console.WriteLine($"{a} Incoming blocks:");
    await foreach (var blockInfo in blocks)
    {
        var blockHash = client.Raw.GetBlockInfo(
            new BlockHashInput()
            {
                Given = new Concordium.V2.BlockHash { Value = blockInfo.Hash.Value }
            }
        );
        Console.WriteLine("Received a block:");
        Console.WriteLine(blockHash.ToString());
    }
}

Callback(1);
Callback(2);

Thread.Sleep(60 * 1000);
