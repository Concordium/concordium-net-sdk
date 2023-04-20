using Concordium.V2;
using Grpc.Core;
using Grpc.Net.Client;

namespace ConcordiumNetSdk;

/// <summary>
/// A client for interacting with the GRPC V2 API exposed by Concordium nodes.
/// </summary>
public class Client : IDisposable
{
    /// <summary>
    /// The "internal" client instance generated from the V2 API protocol buffer definition.
    /// </summary>
    private readonly Queries.QueriesClient _client;

    /// <summary>
    /// The channel on which the client communicates with the Concodium node.
    /// </summary>
    private readonly GrpcChannel _grpcChannel;

    /// <summary>
    /// The configuration to use with the client.
    /// </summary>
    private readonly ClientConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// Uses the <see cref="ClientConfiguration.DefaultConfiguration"/> for the client settings.
    /// </summary>
    /// <param name="url">URL of a resource where the V2 API is served.</param>
    /// <param name="port">Port of the resource where the V2 API is served.</param>
    public Client(string url, UInt16 port)
        : this(url, port, ClientConfiguration.DefaultConfiguration) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="url">URL of a resource where the V2 API is served.</param>
    /// <param name="port">Port of the resource where the V2 API is served.</param>
    /// <param name="timeout">The request timeout in seconds.</param>
    /// <param name="secure">Flag indicating whether the client must use a secure connection.</param>
    public Client(string url, UInt16 port, ulong timeout, bool secure)
        : this(url, port, new ClientConfiguration(timeout, secure)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="url">URL of a resource where the V2 API is served.</param>
    /// <param name="port">Port of the resource where the V2 API is served.</param>
    /// <param name="configuration">The configuration to use with this client.</param>
    public Client(string url, UInt16 port, ClientConfiguration configuration)
    {
        GrpcChannelOptions options = new GrpcChannelOptions
        {
            Credentials = configuration.Secure
                ? ChannelCredentials.SecureSsl
                : ChannelCredentials.Insecure
        };
        _grpcChannel = GrpcChannel.ForAddress(url + ":" + port.ToString(), options);
        _client = new Queries.QueriesClient(_grpcChannel);
        _config = configuration;
    }

    /// <summary>
    /// Process a stream of blocks that arrive from the time the query is made onward.
    /// This can be used to listen for incoming blocks. Note that this is non-terminating.
    /// </summary>
    public IAsyncEnumerable<ArrivedBlockInfo> GetBlocks()
    {
        return _client.GetBlocks(new Empty(), CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Process a stream of blocks that are finalized from the time the query is made onward.
    /// This can be used to listen for newly finalized blocks. Note that this is non-terminating.
    /// </summary>
    public IAsyncEnumerable<FinalizedBlockInfo> GetFinalizedBlocks()
    {
        return _client
            .GetFinalizedBlocks(new Empty(), CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get information about an account.
    /// </summary>
    public AccountInfo GetAccountInfo(AccountInfoRequest input)
    {
        return _client.GetAccountInfo(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information about an account.
    /// </summary>
    public Task<AccountInfo> GetAccountInfoAsync(AccountInfoRequest input)
    {
        return _client.GetAccountInfoAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get all accounts that exist at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<AccountAddress> GetAccountList(BlockHashInput input)
    {
        return _client.GetAccountList(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all smart contract modules that exist at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<ModuleRef> GetModuleList(BlockHashInput input)
    {
        return _client.GetModuleList(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get ancestors of a given block.
    /// The first element of the sequence is the requested block itself, and the block
    /// immediately following a block in the sequence is the parent of that block.
    /// The sequence contains at most @limit@ blocks, and if the sequence is
    /// strictly shorter, the last block in the list is the genesis block.
    /// </summary>
    public IAsyncEnumerable<BlockHash> GetAncestors(AncestorsRequest input)
    {
        return _client.GetAncestors(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get the source of a smart contract module.
    /// </summary>
    public VersionedModuleSource GetModuleSource(ModuleSourceRequest input)
    {
        return _client.GetModuleSource(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns the source of a smart contract module.
    /// </summary>
    public Task<VersionedModuleSource> GetModuleSourceAsync(ModuleSourceRequest input)
    {
        return _client.GetModuleSourceAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get the addresses of all smart contract instances in a given block.
    /// </summary>
    public IAsyncEnumerable<ContractAddress> GetInstanceList(BlockHashInput input)
    {
        return _client.GetInstanceList(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get information about a smart contract instance as it appears at the end of a
    /// given block.
    /// </summary>
    public InstanceInfo GetInstanceInfo(InstanceInfoRequest input)
    {
        return _client.GetInstanceInfo(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information about a smart contract instance as it
    /// appears at the end of a given block.
    /// </summary>
    public Task<InstanceInfo> GetInstanceInfoAsync(InstanceInfoRequest input)
    {
        return _client.GetInstanceInfoAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get key-value pairs representing the entire state of a specific contract instance in a given block.
    /// The resulting sequence consists of key-value pairs ordered lexicographically according to the keys.
    /// </summary>
    public IAsyncEnumerable<InstanceStateKVPair> GetInstanceState(InstanceInfoRequest input)
    {
        return _client.GetInstanceState(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get the value at a specific key of a contract state. In contrast to
    /// <see cref="GetInstanceState"/> this is more efficient, but requires the user to know
    /// the specific key to look up in advance.
    /// </summary>
    public InstanceStateValueAtKey InstanceStateLookup(InstanceStateLookupRequest input)
    {
        return _client.InstanceStateLookup(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns the value at a specific key of a contract state. In contrast to
    /// <see cref="GetInstanceState"/> this is more efficient, but requires the user to know
    /// the specific key to look up in advance.
    /// </summary>
    public Task<InstanceStateValueAtKey> InstanceStateLookupAsync(InstanceStateLookupRequest input)
    {
        return _client.InstanceStateLookupAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get the best guess as to what the next account sequence number (nonce) should be
    /// for the given account.
    /// If all account transactions are finalized then this information is reliable.
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    public NextAccountSequenceNumber GetNextAccountSequenceNumber(AccountAddress input)
    {
        return _client.GetNextAccountSequenceNumber(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns the best guess as to what the next account sequence number (nonce)
    /// should be for the given account.
    /// If all account transactions are finalized then this information is reliable.
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    public Task<NextAccountSequenceNumber> GetNextAccountSequenceNumberAsync(AccountAddress input)
    {
        return _client.GetNextAccountSequenceNumberAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get information about the current state of consensus.
    /// </summary>
    public ConsensusInfo GetConsensusInfo()
    {
        return _client.GetConsensusInfo(new Empty(), CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information about the current state of consensus.
    /// </summary>
    public Task<ConsensusInfo> GetConsensusInfoAsync()
    {
        return _client.GetConsensusInfoAsync(new Empty(), CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get the status of and information about a specific block item (transaction).
    /// </summary>
    public BlockItemStatus GetBlockItemStatus(TransactionHash input)
    {
        return _client.GetBlockItemStatus(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns the status of and information about a specific block item (transaction).
    /// </summary>
    public Task<BlockItemStatus> GetBlockItemStatusAsync(TransactionHash input)
    {
        return _client.GetBlockItemStatusAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get cryptographic parameters in a given block.
    /// </summary>
    public CryptographicParameters GetCryptographicParameters(BlockHashInput input)
    {
        return _client.GetCryptographicParameters(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns the cryptographic parameters in a given block.
    /// </summary>
    public Task<CryptographicParameters> GetCryptographicParametersAsync(BlockHashInput input)
    {
        return _client.GetCryptographicParametersAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get information such as height, timings, and transaction counts for a given block.
    /// </summary>
    public BlockInfo GetBlockInfo(BlockHashInput input)
    {
        return _client.GetBlockInfo(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information such as height, timings, and transaction counts for a given block.
    /// </summary>
    public Task<BlockInfo> GetBlockInfoAsync(BlockHashInput input)
    {
        return _client.GetBlockInfoAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get IDs of all bakers at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<BakerId> GetBakerList(BlockHashInput input)
    {
        return _client.GetBakerList(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get information about a given pool at the end of a given block.
    /// </summary>
    public PoolInfoResponse GetPoolInfo(PoolInfoRequest input)
    {
        return _client.GetPoolInfo(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information about a given pool at the end of a given block.
    /// </summary>
    public Task<PoolInfoResponse> GetPoolInfoAsync(PoolInfoRequest input)
    {
        return _client.GetPoolInfoAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get information about the passive delegators at the end of a given block.
    /// </summary>
    public PassiveDelegationInfo GetPassiveDelegationInfo(BlockHashInput input)
    {
        return _client.GetPassiveDelegationInfo(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information about the passive delegators at the end of a given block.
    /// </summary>
    public Task<PassiveDelegationInfo> GetPassiveDelegationInfoAsync(BlockHashInput input)
    {
        return _client.GetPassiveDelegationInfoAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get a list of live blocks at a given height.
    /// </summary>
    public BlocksAtHeightResponse GetBlocksAtHeight(BlocksAtHeightRequest input)
    {
        return _client.GetBlocksAtHeight(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns a list of live blocks at a given height.
    /// </summary>
    public Task<BlocksAtHeightResponse> GetBlocksAtHeightAsync(BlocksAtHeightRequest input)
    {
        return _client.GetBlocksAtHeightAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get information about tokenomics at the end of a given block.
    /// </summary>
    public TokenomicsInfo GetTokenomicsInfo(BlockHashInput input)
    {
        return _client.GetTokenomicsInfo(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information about tokenomics at the end of a given block.
    /// </summary>
    public Task<TokenomicsInfo> GetTokenomicsInfoAsync(BlockHashInput input)
    {
        return _client.GetTokenomicsInfoAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Instructs the node to run a smart contract entrypoint in the given context and
    /// state at the end of a given block.
    /// </summary>
    public InvokeInstanceResponse InvokeInstance(InvokeInstanceRequest input)
    {
        return _client.InvokeInstance(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which instructs the node to run a smart contract entrypoint in the given context and
    /// state at the end of a given block.
    /// </summary>
    public Task<InvokeInstanceResponse> InvokeInstanceAsync(InvokeInstanceRequest input)
    {
        return _client.InvokeInstanceAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get all registered delegators of a given pool at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorInfo> GetPoolDelegators(GetPoolDelegatorsRequest input)
    {
        return _client.GetPoolDelegators(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all fixed delegators of a given pool for the reward period of a given block.
    /// In contrast to @getPoolDelegators@ which returns all active delegators registered
    /// for the given block, this returns all the active fixed delegators contributing stake
    /// in the reward period containing the given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorRewardPeriodInfo> GetPoolDelegatorsRewardPeriod(
        GetPoolDelegatorsRequest input
    )
    {
        return _client
            .GetPoolDelegatorsRewardPeriod(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all registered passive delegators at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorInfo> GetPassiveDelegators(BlockHashInput input)
    {
        return _client
            .GetPassiveDelegators(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all fixed passive delegators for the reward period of a given block.
    /// In contrast to <see cref="GetPassiveDelegators"/> which returns all delegators registered
    /// at the end of a given block, this returns all fixed delegators contributing
    /// stake in the reward period containing the given block.
    /// </summary>
    public IAsyncEnumerable<DelegatorRewardPeriodInfo> GetPassiveDelegatorsRewardPeriod(
        BlockHashInput input
    )
    {
        return _client
            .GetPassiveDelegatorsRewardPeriod(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get the current branches of blocks starting from and including the last finalized block.
    /// </summary>
    public Branch GetBranches()
    {
        return _client.GetBranches(new Empty(), CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns the current branches of blocks starting from and including the last finalized block.
    /// </summary>
    public Task<Branch> GetBranchesAsync()
    {
        return _client.GetBranchesAsync(new Empty(), CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get information related to the baker election for a particular block.
    /// </summary>
    public ElectionInfo GetElectionInfo(BlockHashInput input)
    {
        return _client.GetElectionInfo(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information related to the baker election for a particular block.
    /// </summary>
    public Task<ElectionInfo> GetElectionInfoAsync(BlockHashInput input)
    {
        return _client.GetElectionInfoAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get all identity providers registered at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<IpInfo> GetIdentityProviders(BlockHashInput input)
    {
        return _client
            .GetIdentityProviders(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all anonymity revokers registered at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<ArInfo> GetAnonymityRevokers(BlockHashInput input)
    {
        return _client
            .GetAnonymityRevokers(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all hashes of non-finalized transactions for a given account.
    /// </summary>
    public IAsyncEnumerable<TransactionHash> GetAccountNonFinalizedTransactions(
        AccountAddress input
    )
    {
        return _client
            .GetAccountNonFinalizedTransactions(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all transaction events in a given block.
    /// </summary>
    public IAsyncEnumerable<BlockItemSummary> GetBlockTransactionEvents(BlockHashInput input)
    {
        return _client
            .GetBlockTransactionEvents(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all special events in a given block.
    /// A special event is protocol generated event that is not directly caused by a transaction, such as minting, paying out rewards, etc.
    /// </summary>
    public IAsyncEnumerable<BlockSpecialEvent> GetBlockSpecialEvents(BlockHashInput input)
    {
        return _client
            .GetBlockSpecialEvents(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Get all pending updates to chain parameters at the end of a given block.
    /// </summary>
    public IAsyncEnumerable<PendingUpdate> GetBlockPendingUpdates(BlockHashInput input)
    {
        return _client
            .GetBlockPendingUpdates(input, CreateCallOptions())
            .ResponseStream.ReadAllAsync();
    }

    public NextUpdateSequenceNumbers GetNextUpdateSequenceNumbers(BlockHashInput input)
    {
        return _client.GetNextUpdateSequenceNumbers(input, CreateCallOptions());
    }

    public Task<NextUpdateSequenceNumbers> GetNextUpdateSequenceNumbersAsync(BlockHashInput input)
    {
        return _client.GetNextUpdateSequenceNumbersAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Request that the node shut down. Throws an exception if the shutdown failed.
    /// </summary>
    public Empty Shutdown()
    {
        return _client.Shutdown(new Empty(), CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task that requests that the node shut down. Throws an exception if the shutdown failed.
    /// </summary>
    public Task<Empty> ShutdownAsync()
    {
        return _client.ShutdownAsync(new Empty(), CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Request that the node connect to the peer with the specified details.
    /// If the request succeeds, the peer is added to the peer-list of the node.
    /// Otherwise a GRPC exception is thrown. Note that the peer may not be connected
    /// instantly, in which case the call will still succeed.
    /// </summary>
    public Empty PeerConnect(IpSocketAddress input)
    {
        return _client.PeerConnect(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which requests that the node connect to the peer with
    /// the specified details.
    /// If the request succeeds, the peer is added to the peer-list of the node.
    /// Otherwise a GRPC exception is thrown. Note that the peer may not be connected
    /// instantly, in which case the call will still succeed.
    /// </summary>
    public Task<Empty> PeerConnectAsync(IpSocketAddress input)
    {
        return _client.PeerConnectAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Request the node to disconnect from the peer with the specified details.
    /// If the request was succesfully processed, the peer is removed from the peer-list.
    /// Otherwise a GRPC exception is returned.
    /// </summary>
    public Empty PeerDisconnect(IpSocketAddress input)
    {
        return _client.PeerDisconnect(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which requests the node to disconnect from the peer with the specified
    /// details. If the request was succesfully processed, the peer is removed from the peer-list.
    /// Otherwise a GRPC exception is returned.
    /// </summary>
    public Task<Empty> PeerDisconnectAsync(IpSocketAddress input)
    {
        return _client.PeerDisconnectAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get a list of peers banned by the node.
    /// </summary>
    public BannedPeers GetBannedPeers()
    {
        return _client.GetBannedPeers(new Empty(), CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns a list of peers banned by the node.
    /// </summary>
    public Task<BannedPeers> GetBannedPeersAsync()
    {
        return _client.GetBannedPeersAsync(new Empty(), CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Request the node to ban the specified peer. Throws a GRPC exception if the action failed.
    /// </summary>
    public Empty BanPeer(PeerToBan input)
    {
        return _client.BanPeer(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which requests the node to ban the specified peer. Throws a GRPC exception if the action failed.
    /// </summary>
    public Task<Empty> BanPeerAsync(PeerToBan input)
    {
        return _client.BanPeerAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Request the node to unban the specified peer. Throws a GRPC error if the action failed.
    /// </summary>
    public Empty UnbanPeer(BannedPeer input)
    {
        return _client.UnbanPeer(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which requests the node to unban the specified peer. Throws a GRPC error if the action failed.
    /// </summary>
    public Task<Empty> UnbanPeerAsync(BannedPeer input)
    {
        return _client.UnbanPeerAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Request the node to start dumping network packets into the specified file.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Returns a GRPC error if the network dump failed to start.
    /// </summary>
    public Empty DumpStart(DumpRequest input)
    {
        return _client.DumpStart(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which requests the node to start dumping network packets into the specified file.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Returns a GRPC error if the network dump failed to start.
    /// </summary>
    public Task<Empty> DumpStartAsync(DumpRequest input)
    {
        return _client.DumpStartAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Request the node to stop dumping packets, if configured to do so.
    /// This feature is enabled if the node was built with the @network_dump@ feature.
    /// Throws a GRPC error if the network dump could not be stopped.
    /// </summary>
    public Empty DumpStop()
    {
        return _client.DumpStop(new Empty(), CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which requests the node to stop dumping packets, if configured to do so.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Throws a GRPC error if the network dump could not be stopped.
    /// </summary>
    public Task<Empty> DumpStopAsync()
    {
        return _client.DumpStopAsync(new Empty(), CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get a list of the peers that the node is connected to as well as network-related information for each such peer.
    /// </summary>
    public PeersInfo GetPeersInfo()
    {
        return _client.GetPeersInfo(new Empty(), CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns a list of the peers that the node is connected to as well as network-related information for each such peer.
    /// </summary>
    public Task<PeersInfo> GetPeersInfoAsync()
    {
        return _client.GetPeersInfoAsync(new Empty(), CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get information about the node.
    /// </summary>
    public NodeInfo GetNodeInfo()
    {
        return _client.GetNodeInfo(new Empty(), CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns information about the node.
    /// </summary>
    public Task<NodeInfo> GetNodeInfoAsync()
    {
        return _client.GetNodeInfoAsync(new Empty(), CreateCallOptions()).ResponseAsync;
    }

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
    public TransactionHash SendBlockItem(SendBlockItemRequest input)
    {
        return _client.SendBlockItem(input, CreateCallOptions());
    }

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
    public Task<TransactionHash> SendBlockItemAsync(SendBlockItemRequest input)
    {
        return _client.SendBlockItemAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get values of block chain parameters in a given block.
    /// </summary>
    public ChainParameters GetBlockChainParameters(BlockHashInput input)
    {
        return _client.GetBlockChainParameters(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns values of block chain parameters in a given block.
    /// </summary>
    public Task<ChainParameters> GetBlockChainParametersAsync(BlockHashInput input)
    {
        return _client.GetBlockChainParametersAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get a summary of the finalization data in a given block.
    /// </summary>
    public BlockFinalizationSummary GetBlockFinalizationSummary(BlockHashInput input)
    {
        return _client.GetBlockFinalizationSummary(input, CreateCallOptions());
    }

    /// <summary>
    /// Spawn a task which returns a summary of the finalization data in a given block.
    /// </summary>
    public Task<BlockFinalizationSummary> GetBlockFinalizationSummaryAsync(BlockHashInput input)
    {
        return _client.GetBlockFinalizationSummaryAsync(input, CreateCallOptions()).ResponseAsync;
    }

    /// <summary>
    /// Get the items of a block.
    /// </summary>
    public IAsyncEnumerable<BlockItem> GetBlockItems(BlockHashInput input)
    {
        return _client.GetBlockItems(input, CreateCallOptions()).ResponseStream.ReadAllAsync();
    }

    /// <summary>
    /// Create the call options for invoking the <see cref="_client">.
    /// </summary>
    private CallOptions CreateCallOptions()
    {
        ulong timeout = this._config.Timeout;
        return new CallOptions(null, DateTime.UtcNow.AddSeconds(timeout), CancellationToken.None);
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
