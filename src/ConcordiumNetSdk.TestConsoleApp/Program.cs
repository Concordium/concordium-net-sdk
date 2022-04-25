using Concordium;
using Grpc.Core;
using Grpc.Net.Client;

var grpcChannel = GrpcChannel.ForAddress("http://localhost:10001");
var client = new P2P.P2PClient(grpcChannel);
var callOptions = new CallOptions(new Metadata {{ "authentication", "rpcadmin" }});

var response = client.GetAccountInfo(
    new GetAddressInfoRequest
    {
        Address = "3rAsvTuH2gQawenRgwJQzrk9t4Kd2Y1uZYinLqJRDAHZKJKEeH",
        BlockHash = "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
    },
    callOptions);

Console.WriteLine(response.Value);