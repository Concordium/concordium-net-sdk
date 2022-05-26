using Concordium;
using ConcordiumNetSdk.Responses.AccountInfoResponse;
using ConcordiumNetSdk.Responses.BlockInfoResponse;
using ConcordiumNetSdk.Responses.ConsensusStatusResponse;
using ConcordiumNetSdk.Responses.NextAccountNonceResponse;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;
using BlockHash = ConcordiumNetSdk.Types.BlockHash;

namespace ConcordiumNetSdk;

public class ConcordiumNodeClient : IConcordiumNodeClient, IDisposable
{
    private readonly P2P.P2PClient _client;
    private readonly Metadata _metadata;
    private readonly GrpcChannel _grpcChannel;

    public ConcordiumNodeClient(Connection connection)
    {
        _metadata = new Metadata
        {
            {"authentication", connection.AuthenticationToken}
        };

        // todo: add all props from here to connection class
        var options = new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Insecure
        };

        _grpcChannel = GrpcChannel.ForAddress(connection.Address, options);
        _client = new P2P.P2PClient(_grpcChannel);
    }

    public async Task<AccountInfo?> GetAccountInfoAsync(AccountAddress accountAddress, BlockHash blockHash)
    {
        var request = new GetAddressInfoRequest
        {
            Address = accountAddress.AsString,
            BlockHash = blockHash.AsString
        };
        JsonResponse response = await _client.GetAccountInfoAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<AccountInfo>(response.Value);
    }

    public async Task<NextAccountNonce?> GetNextAccountNonceAsync(AccountAddress accountAddress)
    {
        var request = new Concordium.AccountAddress
        {
            AccountAddress_ = accountAddress.AsString
        };
        JsonResponse response = await _client.GetNextAccountNonceAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<NextAccountNonce>(response.Value);
    }

    public async Task<ConsensusStatus?> GetConsensusStatusAsync()
    {
        JsonResponse response = await _client.GetConsensusStatusAsync(new Empty(), CreateCallOptions());
        return CustomJsonSerializer.Deserialize<ConsensusStatus>(response.Value);
    }

    public async Task<BlockInfo?> GetBlockInfoAsync(BlockHash blockHash)
    {
        Concordium.BlockHash request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetBlockInfoAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<BlockInfo>(response.Value);
    }

    // todo: think how to implement tests
    public async Task<bool> SendTransactionAsync(byte[] payload, uint networkId = 100)
    {
        var request = new SendTransactionRequest
        {
            NetworkId = networkId,
            Payload = ByteString.CopyFrom(payload)
        };
        BoolResponse response = await _client.SendTransactionAsync(request, CreateCallOptions());
        return response.Value;
    }

    private CallOptions CreateCallOptions()
    {
        return new CallOptions(_metadata, DateTime.UtcNow.AddSeconds(30), CancellationToken.None);
    }

    #region IDisposable Support

    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _grpcChannel.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    #endregion
}
