using Concordium;
using ConcordiumNetSdk.Responses.AccountInfoResponse;
using ConcordiumNetSdk.Responses.NextAccountNonceResponse;
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
