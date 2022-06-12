using Concordium;
using ConcordiumNetSdk.Responses.AccountInfoResponse;
using ConcordiumNetSdk.Responses.AnonymityRevokerInfoResponse;
using ConcordiumNetSdk.Responses.BirkParametersResponse;
using ConcordiumNetSdk.Responses.BlockInfoResponse;
using ConcordiumNetSdk.Responses.BranchResponse;
using ConcordiumNetSdk.Responses.ConsensusStatusResponse;
using ConcordiumNetSdk.Responses.ContractInfoResponse;
using ConcordiumNetSdk.Responses.CryptographicParametersResponse;
using ConcordiumNetSdk.Responses.IdentityProviderInfo;
using ConcordiumNetSdk.Responses.NextAccountNonceResponse;
using ConcordiumNetSdk.Responses.RewardStatusResponse;
using ConcordiumNetSdk.Types;
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

    public async Task<bool> PeerConnectAsync(string ip, int? port = null)
    {
        PeerConnectRequest request = new PeerConnectRequest
        {
            Ip = ip,
            Port = port
        };
        BoolResponse response = await _client.PeerConnectAsync(request, CreateCallOptions());
        return response.Value;
    }

    public async Task<bool> PeerDisconnectAsync(string ip, int? port = null)
    {
        PeerConnectRequest request = new PeerConnectRequest
        {
            Ip = ip,
            Port = port
        };
        BoolResponse response = await _client.PeerDisconnectAsync(request, CreateCallOptions());
        return response.Value;
    }

    public async Task<ulong> GetPeerUptimeAsync()
    {
        NumberResponse response = await _client.PeerUptimeAsync(new Empty(), CreateCallOptions());
        return response.Value;
    }

    public async Task<ulong> GetPeerTotalSentAsync()
    {
        NumberResponse response = await _client.PeerTotalSentAsync(new Empty(), CreateCallOptions());
        return response.Value;
    }

    public async Task<ulong> GetPeerTotalReceivedAsync()
    {
        NumberResponse response = await _client.PeerTotalReceivedAsync(new Empty(), CreateCallOptions());
        return response.Value;
    }

    public async Task<string> GetPeerVersionAsync()
    {
        StringResponse response = await _client.PeerVersionAsync(new Empty(), CreateCallOptions());
        return response.Value;
    }

    public async Task<PeerStatsResponse> GetPeerStatsAsync(bool includeBootstrappers = false)
    {
        PeersRequest request = new PeersRequest
        {
            IncludeBootstrappers = includeBootstrappers
        };
        return await _client.PeerStatsAsync(request, CreateCallOptions());
    }

    public async Task<List<AccountAddress>> GetAccountListAsync(BlockHash blockHash)
    {
        Concordium.BlockHash request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetAccountListAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<List<AccountAddress>>(response.Value) ?? new List<AccountAddress>();
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

    public async Task<List<ContractAddress>> GetInstancesAsync(BlockHash blockHash)
    {
        var request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetInstancesAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<List<ContractAddress>>(response.Value) ?? new List<ContractAddress>();
    }

    public async Task<ContractInfo?> GetInstanceInfoAsync(ContractAddress contractAddress, BlockHash blockHash)
    {
        var request = new GetAddressInfoRequest
        {
            Address = CustomJsonSerializer.Serialize(contractAddress),
            BlockHash = blockHash.AsString
        };
        JsonResponse response = await _client.GetInstanceInfoAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<ContractInfo>(response.Value);
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

    public async Task<RewardStatus?> GetRewardStatusAsync(BlockHash blockHash)
    {
        var request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetRewardStatusAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<RewardStatus>(response.Value);
    }

    public async Task<BirkParameters?> GetBirkParametersAsync(BlockHash blockHash)
    {
        var request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetBirkParametersAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<BirkParameters>(response.Value);
    }

    public async Task<List<ModuleRef>> GetModuleListAsync(BlockHash blockHash)
    {
        var request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetModuleListAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<List<ModuleRef>>(response.Value) ?? new List<ModuleRef>();
    }

    public async Task<ByteString> GetModuleSourceAsync(BlockHash blockHash, ModuleRef moduleRef)
    {
        var request = new GetModuleSourceRequest
        {
            BlockHash = blockHash.AsString,
            ModuleRef = moduleRef.AsString
        };
        BytesResponse response = await _client.GetModuleSourceAsync(request, CreateCallOptions());
        return response.Value;
    }

    public async Task<List<IdentityProviderInfo>> GetIdentityProvidersAsync(BlockHash blockHash)
    {
        var request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetIdentityProvidersAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<List<IdentityProviderInfo>>(response.Value) ?? new List<IdentityProviderInfo>();
    }

    public async Task<List<AnonymityRevokerInfo>> GetAnonymityRevokersAsync(BlockHash blockHash)
    {
        var request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetAnonymityRevokersAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<List<AnonymityRevokerInfo>>(response.Value) ?? new List<AnonymityRevokerInfo>();
    }

    public async Task<VersionedValue<CryptographicParameters>?> GetCryptographicParametersAsync(BlockHash blockHash)
    {
        var request = new Concordium.BlockHash
        {
            BlockHash_ = blockHash.AsString
        };
        JsonResponse response = await _client.GetCryptographicParametersAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<VersionedValue<CryptographicParameters>>(response.Value);
    }

    public async Task<PeerListResponse> GetBannedPeersAsync()
    {
        return await _client.GetBannedPeersAsync(new Empty(), CreateCallOptions());
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

    public async Task<List<BlockHash>> GetAncestorsAsync(ulong amount, BlockHash blockHash)
    {
        BlockHashAndAmount request = new BlockHashAndAmount
        {
            Amount = amount,
            BlockHash = blockHash.AsString
        };
        JsonResponse response = await _client.GetAncestorsAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<List<BlockHash>>(response.Value) ?? new List<BlockHash>();
    }

    public async Task<Branch> GetBranchesAsync()
    {
        JsonResponse response = await _client.GetBranchesAsync(new Empty(), CreateCallOptions());
        return CustomJsonSerializer.Deserialize<Branch>(response.Value);
    }

    public async Task<List<BlockHash>> GetBlocksAtHeightAsync(
        ulong blockHeight,
        uint fromGenesisIndex = 0,
        bool restrictToGenesisIndex = false)
    {
        BlockHeight request = new BlockHeight
        {
            BlockHeight_ = blockHeight,
            FromGenesisIndex = fromGenesisIndex,
            RestrictToGenesisIndex = restrictToGenesisIndex
        };
        JsonResponse response = await _client.GetBlocksAtHeightAsync(request, CreateCallOptions());
        return CustomJsonSerializer.Deserialize<List<BlockHash>>(response.Value) ?? new List<BlockHash>();
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
