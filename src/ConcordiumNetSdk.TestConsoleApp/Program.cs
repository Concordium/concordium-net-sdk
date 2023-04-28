using Newtonsoft.Json;
using Concordium.V2;
using ConcordiumNetSdk;
using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;

// Create the client.
Uri url = new Uri("https://service.internal.stagenet.concordium.com");
UInt16 port = 20000;
Client concordiumNodeClient = new Client(url, port);

var address = "3EqkGQ7NvakjpbUeocRiGzao62ZEEvB6A5rjGsezL8bQBKQGU8";
var mySender = AccountAddress.From(address).ToProto();
var best = new BlockHashInput() { Best = new Empty() };

var myAccountInfoRequest = new AccountInfoRequest()
{
    BlockHash = best,
    AccountIdentifier = new AccountIdentifierInput() { Address = mySender }
};

var moduleListRes = concordiumNodeClient.GetModuleList(best);
var accountInfoRes = concordiumNodeClient.GetAccountInfo(myAccountInfoRequest);

Console.WriteLine("Module list result:");
await foreach (var module in moduleListRes)
{
    Console.WriteLine("Got a module entry in module list stream.");
}

Console.WriteLine("Account info result:");
Console.WriteLine(JsonConvert.SerializeObject(accountInfoRes));
