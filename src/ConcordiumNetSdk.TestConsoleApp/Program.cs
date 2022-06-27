using ConcordiumNetSdk;

Connection connection = new Connection
{
    Address = "http://34.71.98.161:10001",
    AuthenticationToken = "rpcadmin"
};
ConcordiumNodeClient concordiumNodeClient = new ConcordiumNodeClient(connection);


