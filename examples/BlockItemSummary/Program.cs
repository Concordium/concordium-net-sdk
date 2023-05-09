using BlockItemSummary;
using Concordium.Sdk.Client;
using Concordium.Sdk.Types;

Console.WriteLine("Started...");
var endpoint = new Uri("http://node.testnet.concordium.com/");

var client = new ConcordiumClient(endpoint, 20_000);

var transactionHash = TransactionHash.From("6bd564e9e600fd0e5b0e197133a3448c819dfd4f9bcbec956a39d5b2a5029c48");

var blockItemStatus = client.Raw.GetBlockItemStatus(transactionHash.ToProto());

var transactionStatus = TransactionStatusFactory.CreateTransactionStatus(blockItemStatus);

switch (transactionStatus)
{
    case TransactionStatusReceived:
        Console.WriteLine("Received");
        break;
    case TransactionStatusFinalized:
        Console.WriteLine("Finalized");
        break;
    case TransactionStatusCommitted:
        Console.WriteLine("Committed");
        break;
};

Console.WriteLine("{0}", blockItemStatus);