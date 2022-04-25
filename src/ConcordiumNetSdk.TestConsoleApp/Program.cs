using ConcordiumNetSdk;
using ConcordiumNetSdk.Types;

var connection = new Connection
{
    Address = "http://localhost:10001",
    AuthenticationToken = "rpcadmin"
};
var concordiumNodeClient = new ConcordiumNodeClient(connection);

 var accountAddress = AccountAddress.From("32gxbDZj3aCr5RYnKJFkigPazHinKcnAhkxpade17htB4fj6DN");
 var blockHash = "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
var actualAccountInfo = await concordiumNodeClient.GetAccountInfoAsync(accountAddress, blockHash);

Console.WriteLine(actualAccountInfo);