using Concordium;
using Grpc.Core;
using Grpc.Net.Client;
using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;

namespace ConcordiumNetSdk;

public class ConcordiumNodeClient : IDisposable
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

    /// <summary>
    /// Retrieves an information about a state of account corresponding to account address and block hash.
    /// </summary>
    /// <param name="accountAddress">Base-58 check with version byte 1 encoded address (with Bitcoin mapping table).</param>
    /// <param name="blockHash">Base-16 encoded hash of a block (64 characters).</param>
    public async Task<string?> GetAccountInfoAsync(AccountAddress accountAddress, string blockHash)
    {
        var request = new GetAddressInfoRequest
        {
            Address = accountAddress.AsString,
            BlockHash = blockHash
        };
        var response = await _client.GetAccountInfoAsync(request, CreateCallOptions());
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