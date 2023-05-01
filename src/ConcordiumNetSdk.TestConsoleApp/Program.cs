using Newtonsoft.Json;
using Concordium.V2;
using ConcordiumNetSdk.Client;
using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;

// Create the client.
Uri url = new Uri("https://127.0.0.1/");

//Uri url = new Uri("https://172.31.16.30/");
UInt16 port = 8169;
ConcordiumClient concordiumNodeClient = new ConcordiumClient(url, port, 30, false);

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
