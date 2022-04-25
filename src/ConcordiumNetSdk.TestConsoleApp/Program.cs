using ConcordiumNetSdk;

var connection = new Connection
{
    Address = "http://localhost:10001",
    AuthenticationToken = "rpcadmin"
};
var concordiumNodeClient = new ConcordiumNodeClient(connection);