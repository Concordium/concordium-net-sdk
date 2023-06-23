using System.Globalization;
using Concordium.Grpc.V2;
using Grpc.Core;
using Grpc.Net.Client;

namespace Concordium.Sdk.Client;

/// <summary>
/// The raw gRPC client interface exposing wrappers that abstract away
/// connection handling and parameters present in the <see cref="InternalClient"/>
/// which was generated from the protocol buffer definition files for the
/// Concordium Node gRPC API V2.
/// </summary>
public sealed class RawClient : IDisposable
{
    /// <summary>
    /// The maximum permitted duration for a call made by this client, in seconds.
    /// </summary>
    public ulong? Timeout { get; init; }

    /// <summary>
    /// The "internal" client instance generated from the Concordium gRPC API V2 protocol buffer definition.
    /// </summary>
    public Queries.QueriesClient InternalClient { get; init; }

    /// <summary>
    /// The channel on which the client communicates with the Concordium node.
    ///
    /// This reference is needed to implement <see cref="IDisposable"/>.
    /// </summary>
    private readonly GrpcChannel _grpcChannel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RawClient"/> class.
    ///
    /// Optionally use <paramref name="channelOptions"/> to specify connection settings
    /// such as the retry policy or keepalive ping.
    ///
    /// By default the policy is not to retry if a connection could not be established.
    ///
    /// See https://github.com/grpc/grpc/blob/master/doc/keepalive.md for default values
    /// for the keepalive ping parameters.
    /// </summary>
    /// <param name="endpoint">
    /// Endpoint of a resource where the V2 API is served. Any port specified in the URL is
    /// ignored.
    /// </param>
    /// <param name="port">
    /// Port of the resource where the V2 API is served. This will override any port
    /// specified in <paramref name="endpoint"/>.
    /// </param>
    /// <param name="timeout">The maximum permitted duration of a call made by this client, in seconds. <c>null</c> allows the call to run indefinitely.</param>
    /// <param name="channelOptions">The options for the channel that is used to communicate with the node.</param>
    internal RawClient(Uri endpoint, ushort port, ulong? timeout, GrpcChannelOptions? channelOptions)
    {
        // Check the scheme provided in the URI.
        if (!(endpoint.Scheme == Uri.UriSchemeHttp || endpoint.Scheme == Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                $"Unsupported protocol scheme \"{endpoint.Scheme}\" in URL. Expected either \"http\" or \"https\"."
            );
        }

        var scheme = endpoint.Scheme == Uri.UriSchemeHttps
                    ? ChannelCredentials.SecureSsl
                    : ChannelCredentials.Insecure;

        if (channelOptions is null)
        {
            channelOptions = new GrpcChannelOptions
            {
                Credentials = scheme
            };
        }
        else
        {
            channelOptions.Credentials = scheme;
        };

        var grpcChannel = GrpcChannel.ForAddress(
            endpoint.Scheme
                + "://"
                + endpoint.Host
                + ":"
                + port.ToString(CultureInfo.InvariantCulture)
                + endpoint.AbsolutePath,
            channelOptions
        );
        this.Timeout = timeout;
        this.InternalClient = new Queries.QueriesClient(grpcChannel);
        this._grpcChannel = grpcChannel;
    }

    /// <summary>
    /// Process a stream of blocks that arrive from the time the query is made onward.
    /// This can be used to listen for incoming blocks. Note that this is non-terminating,
    /// and that blocks may be skipped if the client is unable to keep up with the stream.
    /// </summary>
    public IAsyncEnumerable<ArrivedBlockInfo> GetBlocks() =>
        this.InternalClient
            .GetBlocks(new Empty(), this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Process a stream of blocks that are finalized from the time the query is made onward.
    /// This can be used to listen for newly finalized blocks. Note that this is non-terminating,
    /// and that blocks may be skipped if the client is unable to keep up with the stream,
    /// however blocks are guaranteed to arrive in order of increasing block height.
    /// </summary>
    public IAsyncEnumerable<FinalizedBlockInfo> GetFinalizedBlocks() =>
        this.InternalClient
            .GetFinalizedBlocks(new Empty(), this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get information about an account.
    /// </summary>
    public AccountInfo GetAccountInfo(AccountInfoRequest input) =>
        this.InternalClient.GetAccountInfo(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information about an account.
    /// </summary>
    public Task<AccountInfo> GetAccountInfoAsync(AccountInfoRequest input) =>
        this.InternalClient.GetAccountInfoAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get all accounts that exist at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<AccountAddress> GetAccountList(BlockHashInput input) =>
        this.InternalClient
            .GetAccountList(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get all smart contract modules that exist at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<ModuleRef> GetModuleList(BlockHashInput input) =>
        this.InternalClient
            .GetModuleList(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get ancestors of a given block.
    /// The first element of the sequence is the requested block itself, and the block
    /// immediately following a block in the sequence is the parent of that block.
    /// The sequence contains at most @limit@ blocks, and if the sequence is
    /// strictly shorter, the last block in the list is the genesis block.
    /// </summary>
    public IAsyncEnumerable<BlockHash> GetAncestors(AncestorsRequest input) =>
        this.InternalClient
            .GetAncestors(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get the source of a smart contract module.
    /// </summary>
    public VersionedModuleSource GetModuleSource(ModuleSourceRequest input) =>
        this.InternalClient.GetModuleSource(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns the source of a smart contract module.
    /// </summary>
    public Task<VersionedModuleSource> GetModuleSourceAsync(ModuleSourceRequest input) =>
        this.InternalClient.GetModuleSourceAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get the addresses of all smart contract instances in a given block.
    /// </summary>
    public IAsyncEnumerable<ContractAddress> GetInstanceList(BlockHashInput input) =>
        this.InternalClient
            .GetInstanceList(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get information about a smart contract instance as it appears at the end of a
    /// given block.
    /// </summary>
    public InstanceInfo GetInstanceInfo(InstanceInfoRequest input) =>
        this.InternalClient.GetInstanceInfo(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information about a smart contract instance as it
    /// appears at the end of a given block.
    /// </summary>
    public Task<InstanceInfo> GetInstanceInfoAsync(InstanceInfoRequest input) =>
        this.InternalClient.GetInstanceInfoAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get key-value pairs representing the entire state of a specific contract instance in a given block.
    /// The resulting sequence consists of key-value pairs ordered lexicographically according to the keys.
    /// </summary>
    public IAsyncEnumerable<InstanceStateKVPair> GetInstanceState(InstanceInfoRequest input) =>
        this.InternalClient
            .GetInstanceState(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get the value at a specific key of a contract state. In contrast to
    /// <see cref="GetInstanceState"/> this is more efficient, but requires the user to know
    /// the specific key to look up in advance.
    /// </summary>
    public InstanceStateValueAtKey InstanceStateLookup(InstanceStateLookupRequest input) =>
        this.InternalClient.InstanceStateLookup(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns the value at a specific key of a contract state. In contrast to
    /// <see cref="GetInstanceState"/> this is more efficient, but requires the user to know
    /// the specific key to look up in advance.
    /// </summary>
    public Task<InstanceStateValueAtKey> InstanceStateLookupAsync(
        InstanceStateLookupRequest input
    ) =>
        this.InternalClient.InstanceStateLookupAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get the best guess as to what the next account sequence number (nonce) should be
    /// for the given account.
    /// If all account transactions are finalized then this information is reliable.
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    public NextAccountSequenceNumber GetNextAccountSequenceNumber(AccountAddress input) =>
        this.InternalClient.GetNextAccountSequenceNumber(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns the best guess as to what the next account sequence number (nonce)
    /// should be for the given account.
    /// If all account transactions are finalized then this information is reliable.
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    public Task<NextAccountSequenceNumber> GetNextAccountSequenceNumberAsync(
        AccountAddress input
    ) =>
        this.InternalClient
            .GetNextAccountSequenceNumberAsync(input, this.CreateCallOptions())
            .ResponseAsync;

    /// <summary>
    /// Get information about the current state of consensus.
    /// </summary>
    public ConsensusInfo GetConsensusInfo() =>
        this.InternalClient.GetConsensusInfo(new Empty(), this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information about the current state of consensus.
    /// </summary>
    public Task<ConsensusInfo> GetConsensusInfoAsync(CancellationToken token = default) =>
        this.InternalClient
            .GetConsensusInfoAsync(new Empty(), this.CreateCallOptions(token))
            .ResponseAsync;

    /// <summary>
    /// Get the status of and information about a specific block item (transaction).
    /// </summary>
    public BlockItemStatus GetBlockItemStatus(TransactionHash input) =>
        this.InternalClient.GetBlockItemStatus(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns the status of and information about a specific block item (transaction).
    /// </summary>
    public Task<BlockItemStatus> GetBlockItemStatusAsync(TransactionHash input) =>
        this.InternalClient.GetBlockItemStatusAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get cryptographic parameters in a given block.
    /// </summary>
    public CryptographicParameters GetCryptographicParameters(BlockHashInput input) =>
        this.InternalClient.GetCryptographicParameters(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns the cryptographic parameters in a given block.
    /// </summary>
    public Task<CryptographicParameters> GetCryptographicParametersAsync(BlockHashInput input) =>
        this.InternalClient
            .GetCryptographicParametersAsync(input, this.CreateCallOptions())
            .ResponseAsync;

    /// <summary>
    /// Get information such as height, timings, and transaction counts for a given block.
    /// </summary>
    public BlockInfo GetBlockInfo(BlockHashInput input) =>
        this.InternalClient.GetBlockInfo(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information such as height, timings, and transaction counts for a given block.
    /// </summary>
    public Task<BlockInfo> GetBlockInfoAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockInfoAsync(input, this.CreateCallOptions(token)).ResponseAsync;

    /// <summary>
    /// Get IDs of all bakers at the end of a given block.
    /// </summary>
    public Task<QueryResponse<IAsyncEnumerable<BakerId>>> GetBakerList(BlockHashInput input)
    {
        var response = this.InternalClient.GetBakerList(input, this.CreateCallOptions());
        return QueryResponse<IAsyncEnumerable<BakerId>>.From(
            response.ResponseHeadersAsync,
            response.ResponseStream.ReadAllAsync());
    }

    /// <summary>
    /// Get information about a given pool at the end of a given block.
    /// </summary>
    public PoolInfoResponse GetPoolInfo(PoolInfoRequest input) =>
        this.InternalClient.GetPoolInfo(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information about a given pool at the end of a given block.
    /// </summary>
    public Task<PoolInfoResponse> GetPoolInfoAsync(PoolInfoRequest input) =>
        this.InternalClient.GetPoolInfoAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get information about the passive delegators at the end of a given block.
    /// </summary>
    public PassiveDelegationInfo GetPassiveDelegationInfo(BlockHashInput input) =>
        this.InternalClient.GetPassiveDelegationInfo(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information about the passive delegators at the end of a given block.
    /// </summary>
    public Task<PassiveDelegationInfo> GetPassiveDelegationInfoAsync(BlockHashInput input) =>
        this.InternalClient
            .GetPassiveDelegationInfoAsync(input, this.CreateCallOptions())
            .ResponseAsync;

    /// <summary>
    /// Get a list of live blocks at a given height.
    /// </summary>
    public BlocksAtHeightResponse GetBlocksAtHeight(BlocksAtHeightRequest input) =>
        this.InternalClient.GetBlocksAtHeight(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns a list of live blocks at a given height.
    /// </summary>
    public Task<BlocksAtHeightResponse> GetBlocksAtHeightAsync(BlocksAtHeightRequest input, CancellationToken token = default) =>
        this.InternalClient.GetBlocksAtHeightAsync(input, this.CreateCallOptions(token)).ResponseAsync;

    /// <summary>
    /// Get information about tokenomics at the end of a given block.
    /// </summary>
    public TokenomicsInfo GetTokenomicsInfo(BlockHashInput input) =>
        this.InternalClient.GetTokenomicsInfo(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information about tokenomics at the end of a given block.
    /// </summary>
    public Task<TokenomicsInfo> GetTokenomicsInfoAsync(BlockHashInput input) =>
        this.InternalClient.GetTokenomicsInfoAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Instructs the node to run a smart contract entrypoint in the given context and
    /// state at the end of a given block.
    /// </summary>
    public InvokeInstanceResponse InvokeInstance(InvokeInstanceRequest input) =>
        this.InternalClient.InvokeInstance(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which instructs the node to run a smart contract entrypoint in the given context and
    /// state at the end of a given block.
    /// </summary>
    public Task<InvokeInstanceResponse> InvokeInstanceAsync(InvokeInstanceRequest input) =>
        this.InternalClient.InvokeInstanceAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get all registered delegators of a given pool at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorInfo> GetPoolDelegators(GetPoolDelegatorsRequest input) =>
        this.InternalClient
            .GetPoolDelegators(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get all fixed delegators of a given pool for the reward period of a given block.
    /// In contrast to @getPoolDelegators@ which returns all active delegators registered
    /// for the given block, this returns all the active fixed delegators contributing stake
    /// in the reward period containing the given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorRewardPeriodInfo> GetPoolDelegatorsRewardPeriod(
        GetPoolDelegatorsRequest input
    ) =>
        this.InternalClient
            .GetPoolDelegatorsRewardPeriod(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get all registered passive delegators at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorInfo> GetPassiveDelegators(BlockHashInput input) =>
        this.InternalClient
            .GetPassiveDelegators(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get all fixed passive delegators for the reward period of a given block.
    /// In contrast to <see cref="GetPassiveDelegators"/> which returns all delegators registered
    /// at the end of a given block, this returns all fixed delegators contributing
    /// stake in the reward period containing the given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorRewardPeriodInfo> GetPassiveDelegatorsRewardPeriod(
        BlockHashInput input
    ) =>
        this.InternalClient
            .GetPassiveDelegatorsRewardPeriod(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get the current branches of blocks starting from and including the last finalized block.
    /// </summary>
    public Branch GetBranches() =>
        this.InternalClient.GetBranches(new Empty(), this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns the current branches of blocks starting from and including the last finalized block.
    /// </summary>
    public Task<Branch> GetBranchesAsync() =>
        this.InternalClient.GetBranchesAsync(new Empty(), this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get information related to the baker election for a particular block.
    /// </summary>
    public ElectionInfo GetElectionInfo(BlockHashInput input) =>
        this.InternalClient.GetElectionInfo(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information related to the baker election for a particular block.
    /// </summary>
    public Task<ElectionInfo> GetElectionInfoAsync(BlockHashInput input) =>
        this.InternalClient.GetElectionInfoAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get all identity providers registered at the end of a given block.
    /// </summary>
    public Task<QueryResponse<IAsyncEnumerable<IpInfo>>> GetIdentityProviders(BlockHashInput input)
    {
        var response = this.InternalClient.GetIdentityProviders(input, this.CreateCallOptions());
        return QueryResponse<IAsyncEnumerable<IpInfo>>.From(
            response.ResponseHeadersAsync,
            response.ResponseStream.ReadAllAsync());
    }

    /// <summary>
    /// Get all anonymity revokers registered at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<ArInfo> GetAnonymityRevokers(BlockHashInput input) =>
        this.InternalClient
            .GetAnonymityRevokers(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get all hashes of non-finalized transactions for a given account.
    /// </summary>
    public IAsyncEnumerable<TransactionHash> GetAccountNonFinalizedTransactions(
        AccountAddress input
    ) =>
        this.InternalClient
            .GetAccountNonFinalizedTransactions(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get all transaction events in a given block.
    /// </summary>
    public IAsyncEnumerable<BlockItemSummary> GetBlockTransactionEvents(BlockHashInput input) =>
        this.InternalClient
            .GetBlockTransactionEvents(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get all special events in a given block.
    /// A special event is protocol generated event that is not directly caused by a transaction, such as minting, paying out rewards, etc.
    /// </summary>
    public IAsyncEnumerable<BlockSpecialEvent> GetBlockSpecialEvents(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetBlockSpecialEvents(input, this.CreateCallOptions(token))
            .ResponseStream.ReadAllAsync(cancellationToken: token);

    /// <summary>
    /// Get all pending updates to chain parameters at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<PendingUpdate> GetBlockPendingUpdates(BlockHashInput input) =>
        this.InternalClient
            .GetBlockPendingUpdates(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Get next available sequence numbers for updating chain parameters after a given block.
    /// </summary>
    public NextUpdateSequenceNumbers GetNextUpdateSequenceNumbers(BlockHashInput input) =>
        this.InternalClient.GetNextUpdateSequenceNumbers(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task that returns the next available sequence numbers for updating chain parameters after a given block.
    /// </summary>
    public Task<NextUpdateSequenceNumbers> GetNextUpdateSequenceNumbersAsync(
        BlockHashInput input
    ) =>
        this.InternalClient
            .GetNextUpdateSequenceNumbersAsync(input, this.CreateCallOptions())
            .ResponseAsync;

    /// <summary>
    /// Request that the node shut down. Throws an exception if the shutdown failed.
    /// </summary>
    public Empty Shutdown() => this.InternalClient.Shutdown(new Empty(), this.CreateCallOptions());

    /// <summary>
    /// Spawn a task that requests that the node shut down. Throws an exception if the shutdown failed.
    /// </summary>
    public Task<Empty> ShutdownAsync() =>
        this.InternalClient.ShutdownAsync(new Empty(), this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Request that the node connect to the peer with the specified details.
    /// If the request succeeds, the peer is added to the peer-list of the node.
    /// Otherwise a gRPC exception is thrown. Note that the peer may not be connected
    /// instantly, in which case the call will still succeed.
    /// </summary>
    public Empty PeerConnect(IpSocketAddress input) =>
        this.InternalClient.PeerConnect(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which requests that the node connect to the peer with
    /// the specified details.
    /// If the request succeeds, the peer is added to the peer-list of the node.
    /// Otherwise a gRPC exception is thrown. Note that the peer may not be connected
    /// instantly, in which case the call will still succeed.
    /// </summary>
    public Task<Empty> PeerConnectAsync(IpSocketAddress input) =>
        this.InternalClient.PeerConnectAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Request the node to disconnect from the peer with the specified details.
    /// If the request was succesfully processed, the peer is removed from the peer-list.
    /// Otherwise a gRPC exception is returned.
    /// </summary>
    public Empty PeerDisconnect(IpSocketAddress input) =>
        this.InternalClient.PeerDisconnect(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which requests the node to disconnect from the peer with the specified
    /// details. If the request was succesfully processed, the peer is removed from the peer-list.
    /// Otherwise a gRPC exception is returned.
    /// </summary>
    public Task<Empty> PeerDisconnectAsync(IpSocketAddress input) =>
        this.InternalClient.PeerDisconnectAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get a list of peers banned by the node.
    /// </summary>
    public BannedPeers GetBannedPeers() =>
        this.InternalClient.GetBannedPeers(new Empty(), this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns a list of peers banned by the node.
    /// </summary>
    public Task<BannedPeers> GetBannedPeersAsync() =>
        this.InternalClient
            .GetBannedPeersAsync(new Empty(), this.CreateCallOptions())
            .ResponseAsync;

    /// <summary>
    /// Request the node to ban the specified peer. Throws a gRPC exception if the action failed.
    /// </summary>
    public Empty BanPeer(PeerToBan input) =>
        this.InternalClient.BanPeer(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which requests the node to ban the specified peer. Throws a gRPC exception if the action failed.
    /// </summary>
    public Task<Empty> BanPeerAsync(PeerToBan input) =>
        this.InternalClient.BanPeerAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Request the node to unban the specified peer. Throws a gRPC error if the action failed.
    /// </summary>
    public Empty UnbanPeer(BannedPeer input) =>
        this.InternalClient.UnbanPeer(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which requests the node to unban the specified peer. Throws a gRPC error if the action failed.
    /// </summary>
    public Task<Empty> UnbanPeerAsync(BannedPeer input) =>
        this.InternalClient.UnbanPeerAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Request the node to start dumping network packets into the specified file.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Returns a gRPC error if the network dump failed to start.
    /// </summary>
    public Empty DumpStart(DumpRequest input) =>
        this.InternalClient.DumpStart(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which requests the node to start dumping network packets into the specified file.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Returns a gRPC error if the network dump failed to start.
    /// </summary>
    public Task<Empty> DumpStartAsync(DumpRequest input) =>
        this.InternalClient.DumpStartAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Request the node to stop dumping packets, if configured to do so.
    /// This feature is enabled if the node was built with the @network_dump@ feature.
    /// Throws a gRPC error if the network dump could not be stopped.
    /// </summary>
    public Empty DumpStop() => this.InternalClient.DumpStop(new Empty(), this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which requests the node to stop dumping packets, if configured to do so.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Throws a gRPC error if the network dump could not be stopped.
    /// </summary>
    public Task<Empty> DumpStopAsync() =>
        this.InternalClient.DumpStopAsync(new Empty(), this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get a list of the peers that the node is connected to as well as network-related information for each such peer.
    /// </summary>
    public PeersInfo GetPeersInfo() =>
        this.InternalClient.GetPeersInfo(new Empty(), this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns a list of the peers that the node is connected to as well as network-related information for each such peer.
    /// </summary>
    public Task<PeersInfo> GetPeersInfoAsync() =>
        this.InternalClient.GetPeersInfoAsync(new Empty(), this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get information about the node.
    /// </summary>
    public NodeInfo GetNodeInfo() =>
        this.InternalClient.GetNodeInfo(new Empty(), this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns information about the node.
    /// </summary>
    public Task<NodeInfo> GetNodeInfoAsync() =>
        this.InternalClient.GetNodeInfoAsync(new Empty(), this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Send a block item to the node. A block item is either an account transaction,
    /// which is a transaction signed and paid for by an account, a credential deployment,
    /// which creates a new account, or an update instruction, which is an instruction
    /// to change some parameters of the chain. Update instructions can only be sent by
    /// the governance committee.
    ///
    /// Returns a hash of the sent block item, which can be used with
    /// <see cref="GetBlockItemStatus"/>.
    /// </summary>
    public TransactionHash SendBlockItem(SendBlockItemRequest input) =>
        this.InternalClient.SendBlockItem(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which send a block item to the node. A block item is either an
    /// account transaction, which is a transaction signed and paid for by an account,
    /// a credential deployment, which creates a new account, or an update instruction,
    /// which is an instruction to change some parameters of the chain. Update instructions
    /// can only be sent by the governance committee.
    ///
    /// Returns a hash of the sent block item, which can be used with
    /// <see cref="GetBlockItemStatus"/>.
    /// </summary>
    public Task<TransactionHash> SendBlockItemAsync(SendBlockItemRequest input) =>
        this.InternalClient.SendBlockItemAsync(input, this.CreateCallOptions()).ResponseAsync;

    /// <summary>
    /// Get values of block chain parameters in a given block.
    /// </summary>
    public ChainParameters GetBlockChainParameters(BlockHashInput input) =>
        this.InternalClient.GetBlockChainParameters(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns values of block chain parameters in a given block.
    /// </summary>
    public async Task<QueryResponse<ChainParameters>> GetBlockChainParametersAsync(BlockHashInput input, CancellationToken token = default)
    {
        var response = this.InternalClient
            .GetBlockChainParametersAsync(input, this.CreateCallOptions(token));

        var chainParameters = await response.ResponseAsync
            .ConfigureAwait(false);

        return await QueryResponse<ChainParameters>.From(response.ResponseHeadersAsync, chainParameters)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get a summary of the finalization data in a given block.
    /// </summary>
    public BlockFinalizationSummary GetBlockFinalizationSummary(BlockHashInput input) =>
        this.InternalClient.GetBlockFinalizationSummary(input, this.CreateCallOptions());

    /// <summary>
    /// Spawn a task which returns a summary of the finalization data in a given block.
    /// </summary>
    public Task<BlockFinalizationSummary> GetBlockFinalizationSummaryAsync(BlockHashInput input) =>
        this.InternalClient
            .GetBlockFinalizationSummaryAsync(input, this.CreateCallOptions())
            .ResponseAsync;

    /// <summary>
    /// Get the items of a block.
    /// </summary>
    public IAsyncEnumerable<BlockItem> GetBlockItems(BlockHashInput input) =>
        this.InternalClient
            .GetBlockItems(input, this.CreateCallOptions())
            .ResponseStream.ReadAllAsync();

    /// <summary>
    /// Create the call options for invoking the <see cref="InternalClient">.
    /// </summary>
    private CallOptions CreateCallOptions(CancellationToken token = default)
    {
        DateTime? deadline;
        if (this.Timeout is null)
        {
            deadline = null;
        }
        else
        {
            deadline = DateTime.UtcNow.AddSeconds((double)this.Timeout);
        }
        return new CallOptions(null, deadline, token);
    }

    public void Dispose() => this._grpcChannel.Dispose();
}
