using Concordium;
using Grpc.Core;
using Grpc.Net.Client;

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