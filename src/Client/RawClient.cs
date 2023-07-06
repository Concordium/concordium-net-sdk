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
    /// Options used by the client at initialization.
    /// Some parameters, like <see cref="GrpcChannelOptions.Credentials"/>, may be set if null when given to constructor.
    /// Hence properties in this are not neccessary equal to those given to constructor, but they equals those used for
    /// client initialization.
    /// </summary>
    public ConcordiumClientOptions Options { get; init; }

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
    /// DEPRECATED - Use overload which accepts <see cref="ConcordiumClientOptions"/>
    ///
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
    [Obsolete($"Use {nameof(RawClient)} with overloads which accepts {nameof(ConcordiumClientOptions)}")]
    internal RawClient(Uri endpoint, ushort port, ulong? timeout, GrpcChannelOptions? channelOptions)
    {
        var scheme = GetEndpointScheme(endpoint);

        channelOptions = SetSchemeIfNotSet(channelOptions, scheme);

        var grpcChannel = GrpcChannel.ForAddress(
            endpoint.Scheme
            + "://"
            + endpoint.Host
            + ":"
            + port.ToString(CultureInfo.InvariantCulture)
            + endpoint.AbsolutePath,
            channelOptions
        );

        this.Options = new ConcordiumClientOptions
        {
            Timeout = timeout.HasValue ? TimeSpan.FromSeconds((double)timeout) : null,
            ChannelOptions = channelOptions
        };
        this.InternalClient = new Queries.QueriesClient(grpcChannel);
        this._grpcChannel = grpcChannel;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RawClient"/> class.
    /// </summary>
    /// <param name="endpoint">Endpoint required by the client.</param>
    /// <param name="options">Options needed for initialization of client.</param>
    internal RawClient(Uri endpoint, ConcordiumClientOptions options)
    {
        var scheme = GetEndpointScheme(endpoint);

        var channelOptions = SetSchemeIfNotSet(options.ChannelOptions, scheme);

        var grpcChannel = GrpcChannel.ForAddress(endpoint, channelOptions);

        this.Options = options;
        this.InternalClient = new Queries.QueriesClient(grpcChannel);
        this._grpcChannel = grpcChannel;
    }

    /// <summary>
    /// Process a stream of blocks that arrive from the time the query is made onward.
    /// This can be used to listen for incoming blocks. Note that this is non-terminating,
    /// and that blocks may be skipped if the client is unable to keep up with the stream.
    /// </summary>
    public AsyncServerStreamingCall<ArrivedBlockInfo> GetBlocks(CancellationToken token = default) =>
        this.InternalClient
            .GetBlocks(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Process a stream of blocks that are finalized from the time the query is made onward.
    /// This can be used to listen for newly finalized blocks. Note that this is non-terminating,
    /// and that blocks may be skipped if the client is unable to keep up with the stream,
    /// however blocks are guaranteed to arrive in order of increasing block height.
    /// </summary>
    public AsyncServerStreamingCall<FinalizedBlockInfo> GetFinalizedBlocks(CancellationToken token = default) =>
        this.InternalClient
            .GetFinalizedBlocks(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Get information about an account.
    /// </summary>
    public AccountInfo GetAccountInfo(AccountInfoRequest input, CancellationToken token = default) =>
        this.InternalClient.GetAccountInfo(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns information about an account.
    /// </summary>
    public AsyncUnaryCall<AccountInfo> GetAccountInfoAsync(AccountInfoRequest input, CancellationToken token = default) =>
        this.InternalClient.GetAccountInfoAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all accounts that exist at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<AccountAddress> GetAccountList(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetAccountList(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all smart contract modules that exist at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<ModuleRef> GetModuleList(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetModuleList(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get ancestors of a given block.
    /// The first element of the sequence is the requested block itself, and the block
    /// immediately following a block in the sequence is the parent of that block.
    /// The sequence contains at most @limit@ blocks, and if the sequence is
    /// strictly shorter, the last block in the list is the genesis block.
    /// </summary>
    public AsyncServerStreamingCall<BlockHash> GetAncestors(AncestorsRequest input, CancellationToken token = default) =>
        this.InternalClient
            .GetAncestors(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get the source of a smart contract module.
    /// </summary>
    public VersionedModuleSource GetModuleSource(ModuleSourceRequest input, CancellationToken token = default) =>
        this.InternalClient.GetModuleSource(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns the source of a smart contract module.
    /// </summary>
    public AsyncUnaryCall<VersionedModuleSource> GetModuleSourceAsync(ModuleSourceRequest input, CancellationToken token = default) =>
        this.InternalClient.GetModuleSourceAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get the addresses of all smart contract instances in a given block.
    /// </summary>
    public AsyncServerStreamingCall<ContractAddress> GetInstanceList(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetInstanceList(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get information about a smart contract instance as it appears at the end of a
    /// given block.
    /// </summary>
    public InstanceInfo GetInstanceInfo(InstanceInfoRequest input, CancellationToken token = default) =>
        this.InternalClient.GetInstanceInfo(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns information about a smart contract instance as it
    /// appears at the end of a given block.
    /// </summary>
    public AsyncUnaryCall<InstanceInfo> GetInstanceInfoAsync(InstanceInfoRequest input, CancellationToken token = default) =>
        this.InternalClient.GetInstanceInfoAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get key-value pairs representing the entire state of a specific contract instance in a given block.
    /// The resulting sequence consists of key-value pairs ordered lexicographically according to the keys.
    /// </summary>
    public AsyncServerStreamingCall<InstanceStateKVPair> GetInstanceState(InstanceInfoRequest input, CancellationToken token = default) =>
        this.InternalClient
            .GetInstanceState(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get the value at a specific key of a contract state. In contrast to
    /// <see cref="GetInstanceState"/> this is more efficient, but requires the user to know
    /// the specific key to look up in advance.
    /// </summary>
    public InstanceStateValueAtKey InstanceStateLookup(InstanceStateLookupRequest input, CancellationToken token = default) =>
        this.InternalClient.InstanceStateLookup(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns the value at a specific key of a contract state. In contrast to
    /// <see cref="GetInstanceState"/> this is more efficient, but requires the user to know
    /// the specific key to look up in advance.
    /// </summary>
    public AsyncUnaryCall<InstanceStateValueAtKey> InstanceStateLookupAsync(
        InstanceStateLookupRequest input, CancellationToken token = default
    ) =>
        this.InternalClient.InstanceStateLookupAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get the best guess as to what the next account sequence number (nonce) should be
    /// for the given account.
    /// If all account transactions are finalized then this information is reliable.
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    public NextAccountSequenceNumber GetNextAccountSequenceNumber(AccountAddress input, CancellationToken token = default) =>
        this.InternalClient.GetNextAccountSequenceNumber(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns the best guess as to what the next account sequence number (nonce)
    /// should be for the given account.
    /// If all account transactions are finalized then this information is reliable.
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    public AsyncUnaryCall<NextAccountSequenceNumber> GetNextAccountSequenceNumberAsync(
        AccountAddress input, CancellationToken token = default
    ) =>
        this.InternalClient
            .GetNextAccountSequenceNumberAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get information about the current state of consensus.
    /// </summary>
    public ConsensusInfo GetConsensusInfo(CancellationToken token = default) =>
        this.InternalClient.GetConsensusInfo(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Returns information about the current state of consensus.
    /// </summary>
    public AsyncUnaryCall<ConsensusInfo> GetConsensusInfoAsync(CancellationToken token = default) =>
        this.InternalClient
            .GetConsensusInfoAsync(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Get the status of and information about a specific block item (transaction).
    /// </summary>
    public BlockItemStatus GetBlockItemStatus(TransactionHash input, CancellationToken token = default) =>
        this.InternalClient.GetBlockItemStatus(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns the status of and information about a specific block item (transaction).
    /// </summary>
    public AsyncUnaryCall<BlockItemStatus> GetBlockItemStatusAsync(TransactionHash input, CancellationToken token = default) =>
        this.InternalClient.GetBlockItemStatusAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get cryptographic parameters in a given block.
    /// </summary>
    public CryptographicParameters GetCryptographicParameters(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetCryptographicParameters(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns the cryptographic parameters in a given block.
    /// </summary>
    public AsyncUnaryCall<CryptographicParameters> GetCryptographicParametersAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetCryptographicParametersAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get information such as height, timings, and transaction counts for a given block.
    /// </summary>
    public BlockInfo GetBlockInfo(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockInfo(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns information such as height, timings, and transaction counts for a given block.
    /// </summary>
    public AsyncUnaryCall<BlockInfo> GetBlockInfoAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockInfoAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get IDs of all bakers at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<BakerId> GetBakerList(BlockHashInput input, CancellationToken token = default)
        => this.InternalClient.GetBakerList(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get information about a given pool at the end of a given block.
    /// </summary>
    public PoolInfoResponse GetPoolInfo(PoolInfoRequest input, CancellationToken token = default) =>
        this.InternalClient.GetPoolInfo(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns information about a given pool at the end of a given block.
    /// </summary>
    public AsyncUnaryCall<PoolInfoResponse> GetPoolInfoAsync(PoolInfoRequest input, CancellationToken token = default) =>
        this.InternalClient.GetPoolInfoAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get information about the passive delegators at the end of a given block.
    /// </summary>
    public PassiveDelegationInfo GetPassiveDelegationInfo(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetPassiveDelegationInfo(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns information about the passive delegators at the end of a given block.
    /// </summary>
    public AsyncUnaryCall<PassiveDelegationInfo> GetPassiveDelegationInfoAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetPassiveDelegationInfoAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get a list of live blocks at a given height.
    /// </summary>
    public BlocksAtHeightResponse GetBlocksAtHeight(BlocksAtHeightRequest input, CancellationToken token = default) =>
        this.InternalClient.GetBlocksAtHeight(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns a list of live blocks at a given height.
    /// </summary>
    public AsyncUnaryCall<BlocksAtHeightResponse> GetBlocksAtHeightAsync(BlocksAtHeightRequest input, CancellationToken token = default) =>
        this.InternalClient.GetBlocksAtHeightAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get information about tokenomics at the end of a given block.
    /// </summary>
    public TokenomicsInfo GetTokenomicsInfo(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetTokenomicsInfo(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns information about tokenomics at the end of a given block.
    /// </summary>
    public AsyncUnaryCall<TokenomicsInfo> GetTokenomicsInfoAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetTokenomicsInfoAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Instructs the node to run a smart contract entrypoint in the given context and
    /// state at the end of a given block.
    /// </summary>
    public InvokeInstanceResponse InvokeInstance(InvokeInstanceRequest input, CancellationToken token = default) =>
        this.InternalClient.InvokeInstance(input, this.CreateCallOptions(token));

    /// <summary>
    /// Instructs the node to run a smart contract entrypoint in the given context and
    /// state at the end of a given block.
    /// </summary>
    public AsyncUnaryCall<InvokeInstanceResponse> InvokeInstanceAsync(InvokeInstanceRequest input, CancellationToken token = default) =>
        this.InternalClient.InvokeInstanceAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all registered delegators of a given pool at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<DelegatorInfo> GetPoolDelegators(GetPoolDelegatorsRequest input, CancellationToken token = default) =>
        this.InternalClient
            .GetPoolDelegators(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all fixed delegators of a given pool for the reward period of a given block.
    /// In contrast to @getPoolDelegators@ which returns all active delegators registered
    /// for the given block, this returns all the active fixed delegators contributing stake
    /// in the reward period containing the given block.
    /// </summary>
    public AsyncServerStreamingCall<DelegatorRewardPeriodInfo> GetPoolDelegatorsRewardPeriod(
        GetPoolDelegatorsRequest input, CancellationToken token = default
    ) =>
        this.InternalClient
            .GetPoolDelegatorsRewardPeriod(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all registered passive delegators at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<DelegatorInfo> GetPassiveDelegators(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetPassiveDelegators(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all fixed passive delegators for the reward period of a given block.
    /// In contrast to <see cref="GetPassiveDelegators"/> which returns all delegators registered
    /// at the end of a given block, this returns all fixed delegators contributing
    /// stake in the reward period containing the given block.
    /// </summary>
    public AsyncServerStreamingCall<DelegatorRewardPeriodInfo> GetPassiveDelegatorsRewardPeriod(
        BlockHashInput input, CancellationToken token = default
    ) =>
        this.InternalClient
            .GetPassiveDelegatorsRewardPeriod(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get the current branches of blocks starting from and including the last finalized block.
    /// </summary>
    public Branch GetBranches(CancellationToken token = default) =>
        this.InternalClient.GetBranches(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Returns the current branches of blocks starting from and including the last finalized block.
    /// </summary>
    public AsyncUnaryCall<Branch> GetBranchesAsync(CancellationToken token = default) =>
        this.InternalClient.GetBranchesAsync(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Get information related to the baker election for a particular block.
    /// </summary>
    public ElectionInfo GetElectionInfo(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetElectionInfo(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns information related to the baker election for a particular block.
    /// </summary>
    public AsyncUnaryCall<ElectionInfo> GetElectionInfoAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetElectionInfoAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all identity providers registered at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<IpInfo> GetIdentityProviders(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetIdentityProviders(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all anonymity revokers registered at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<ArInfo> GetAnonymityRevokers(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetAnonymityRevokers(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all hashes of non-finalized transactions for a given account.
    /// </summary>
    public AsyncServerStreamingCall<TransactionHash> GetAccountNonFinalizedTransactions(AccountAddress input, CancellationToken token = default) =>
        this.InternalClient.GetAccountNonFinalizedTransactions(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all transaction events in a given block.
    /// </summary>
    public AsyncServerStreamingCall<BlockItemSummary> GetBlockTransactionEvents(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockTransactionEvents(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all special events in a given block.
    /// A special event is protocol generated event that is not directly caused by a transaction, such as minting, paying out rewards, etc.
    /// </summary>
    public AsyncServerStreamingCall<BlockSpecialEvent> GetBlockSpecialEvents(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockSpecialEvents(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get all pending updates to chain parameters at the end of a given block.
    /// </summary>
    public AsyncServerStreamingCall<PendingUpdate> GetBlockPendingUpdates(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockPendingUpdates(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get next available sequence numbers for updating chain parameters after a given block.
    /// </summary>
    public NextUpdateSequenceNumbers GetNextUpdateSequenceNumbers(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetNextUpdateSequenceNumbers(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns the next available sequence numbers for updating chain parameters after a given block.
    /// </summary>
    public AsyncUnaryCall<NextUpdateSequenceNumbers> GetNextUpdateSequenceNumbersAsync(
        BlockHashInput input, CancellationToken token = default
    ) =>
        this.InternalClient
            .GetNextUpdateSequenceNumbersAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Request that the node shut down. Throws an exception if the shutdown failed.
    /// </summary>
    public Empty Shutdown(CancellationToken token = default) => this.InternalClient.Shutdown(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Requests that the node shut down. Throws an exception if the shutdown failed.
    /// </summary>
    public AsyncUnaryCall<Empty> ShutdownAsync(CancellationToken token = default) =>
        this.InternalClient.ShutdownAsync(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Request that the node connect to the peer with the specified details.
    /// If the request succeeds, the peer is added to the peer-list of the node.
    /// Otherwise a gRPC exception is thrown. Note that the peer may not be connected
    /// instantly, in which case the call will still succeed.
    /// </summary>
    public Empty PeerConnect(IpSocketAddress input, CancellationToken token = default) =>
        this.InternalClient.PeerConnect(input, this.CreateCallOptions(token));

    /// <summary>
    /// Requests that the node connect to the peer with
    /// the specified details.
    /// If the request succeeds, the peer is added to the peer-list of the node.
    /// Otherwise a gRPC exception is thrown. Note that the peer may not be connected
    /// instantly, in which case the call will still succeed.
    /// </summary>
    public AsyncUnaryCall<Empty> PeerConnectAsync(IpSocketAddress input, CancellationToken token = default) =>
        this.InternalClient.PeerConnectAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Request the node to disconnect from the peer with the specified details.
    /// If the request was succesfully processed, the peer is removed from the peer-list.
    /// Otherwise a gRPC exception is returned.
    /// </summary>
    public Empty PeerDisconnect(IpSocketAddress input, CancellationToken token = default) =>
        this.InternalClient.PeerDisconnect(input, this.CreateCallOptions(token));

    /// <summary>
    /// Requests the node to disconnect from the peer with the specified
    /// details. If the request was succesfully processed, the peer is removed from the peer-list.
    /// Otherwise a gRPC exception is returned.
    /// </summary>
    public AsyncUnaryCall<Empty> PeerDisconnectAsync(IpSocketAddress input, CancellationToken token = default) =>
        this.InternalClient.PeerDisconnectAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get a list of peers banned by the node.
    /// </summary>
    public BannedPeers GetBannedPeers(CancellationToken token = default) =>
        this.InternalClient.GetBannedPeers(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Returns a list of peers banned by the node.
    /// </summary>
    public AsyncUnaryCall<BannedPeers> GetBannedPeersAsync(CancellationToken token = default) =>
        this.InternalClient
            .GetBannedPeersAsync(new Empty(), this.CreateCallOptions(token))
            ;

    /// <summary>
    /// Request the node to ban the specified peer. Throws a gRPC exception if the action failed.
    /// </summary>
    public Empty BanPeer(PeerToBan input, CancellationToken token = default) =>
        this.InternalClient.BanPeer(input, this.CreateCallOptions(token));

    /// <summary>
    /// Requests the node to ban the specified peer. Throws a gRPC exception if the action failed.
    /// </summary>
    public AsyncUnaryCall<Empty> BanPeerAsync(PeerToBan input, CancellationToken token = default) =>
        this.InternalClient.BanPeerAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Request the node to unban the specified peer. Throws a gRPC error if the action failed.
    /// </summary>
    public Empty UnbanPeer(BannedPeer input, CancellationToken token = default) =>
        this.InternalClient.UnbanPeer(input, this.CreateCallOptions(token));

    /// <summary>
    /// Requests the node to unban the specified peer. Throws a gRPC error if the action failed.
    /// </summary>
    public AsyncUnaryCall<Empty> UnbanPeerAsync(BannedPeer input, CancellationToken token = default) =>
        this.InternalClient.UnbanPeerAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Request the node to start dumping network packets into the specified file.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Returns a gRPC error if the network dump failed to start.
    /// </summary>
    public Empty DumpStart(DumpRequest input, CancellationToken token = default) =>
        this.InternalClient.DumpStart(input, this.CreateCallOptions(token));

    /// <summary>
    /// Requests the node to start dumping network packets into the specified file.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Returns a gRPC error if the network dump failed to start.
    /// </summary>
    public AsyncUnaryCall<Empty> DumpStartAsync(DumpRequest input, CancellationToken token = default) =>
        this.InternalClient.DumpStartAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Request the node to stop dumping packets, if configured to do so.
    /// This feature is enabled if the node was built with the @network_dump@ feature.
    /// Throws a gRPC error if the network dump could not be stopped.
    /// </summary>
    public Empty DumpStop(CancellationToken token = default) => this.InternalClient.DumpStop(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Requests the node to stop dumping packets, if configured to do so.
    /// This feature is enabled if the node was built with the <c>network_dump</c> feature.
    /// Throws a gRPC error if the network dump could not be stopped.
    /// </summary>
    public AsyncUnaryCall<Empty> DumpStopAsync(CancellationToken token = default) =>
        this.InternalClient.DumpStopAsync(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Get a list of the peers that the node is connected to as well as network-related information for each such peer.
    /// </summary>
    public PeersInfo GetPeersInfo(CancellationToken token = default) =>
        this.InternalClient.GetPeersInfo(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Returns a list of the peers that the node is connected to as well as network-related information for each such peer.
    /// </summary>
    public AsyncUnaryCall<PeersInfo> GetPeersInfoAsync(CancellationToken token = default) =>
        this.InternalClient.GetPeersInfoAsync(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Get information about the node.
    /// </summary>
    public NodeInfo GetNodeInfo(CancellationToken token = default) =>
        this.InternalClient.GetNodeInfo(new Empty(), this.CreateCallOptions(token));

    /// <summary>
    /// Returns information about the node.
    /// </summary>
    public AsyncUnaryCall<NodeInfo> GetNodeInfoAsync(CancellationToken token = default) =>
        this.InternalClient.GetNodeInfoAsync(new Empty(), this.CreateCallOptions(token));

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
    public TransactionHash SendBlockItem(SendBlockItemRequest input, CancellationToken token = default) =>
        this.InternalClient.SendBlockItem(input, this.CreateCallOptions(token));

    /// <summary>
    /// Send a block item to the node. A block item is either an
    /// account transaction, which is a transaction signed and paid for by an account,
    /// a credential deployment, which creates a new account, or an update instruction,
    /// which is an instruction to change some parameters of the chain. Update instructions
    /// can only be sent by the governance committee.
    ///
    /// Returns a hash of the sent block item, which can be used with
    /// <see cref="GetBlockItemStatus"/>.
    /// </summary>
    public AsyncUnaryCall<TransactionHash> SendBlockItemAsync(SendBlockItemRequest input, CancellationToken token = default) =>
        this.InternalClient.SendBlockItemAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get values of block chain parameters in a given block.
    /// </summary>
    public ChainParameters GetBlockChainParameters(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockChainParameters(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns values of block chain parameters in a given block.
    /// </summary>
    public AsyncUnaryCall<ChainParameters> GetBlockChainParametersAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockChainParametersAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get a summary of the finalization data in a given block.
    /// </summary>
    public BlockFinalizationSummary GetBlockFinalizationSummary(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockFinalizationSummary(input, this.CreateCallOptions(token));

    /// <summary>
    /// Returns a summary of the finalization data in a given block.
    /// </summary>
    public AsyncUnaryCall<BlockFinalizationSummary> GetBlockFinalizationSummaryAsync(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient
            .GetBlockFinalizationSummaryAsync(input, this.CreateCallOptions(token));

    /// <summary>
    /// Get the items of a block.
    /// </summary>
    public AsyncServerStreamingCall<BlockItem> GetBlockItems(BlockHashInput input, CancellationToken token = default) =>
        this.InternalClient.GetBlockItems(input, this.CreateCallOptions(token));

    /// <summary>
    /// Create the call options for invoking the <see cref="InternalClient"/>.
    /// </summary>
    private CallOptions CreateCallOptions(CancellationToken token)
    {
        DateTime? deadline = this.Options.Timeout.HasValue ?
            DateTime.UtcNow.Add(this.Options.Timeout.Value) :
            null;

        return new CallOptions(null, deadline, token);
    }

    /// <summary>
    /// Get credentials from uri scheme.
    /// </summary>
    /// <exception cref="ArgumentException">When scheme not supported..</exception>
    private static ChannelCredentials GetEndpointScheme(Uri endpoint)
    {
        if (!(endpoint.Scheme == Uri.UriSchemeHttp || endpoint.Scheme == Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                $"Unsupported protocol scheme \"{endpoint.Scheme}\" in URL. Expected either \"http\" or \"https\"."
            );
        }
        return endpoint.Scheme == Uri.UriSchemeHttps
                    ? ChannelCredentials.SecureSsl
                    : ChannelCredentials.Insecure;
    }

    private static GrpcChannelOptions SetSchemeIfNotSet(GrpcChannelOptions? channelOptions, ChannelCredentials scheme)
    {
        if (channelOptions is null)
        {
            channelOptions = new GrpcChannelOptions
            {
                Credentials = scheme
            };
        }
        else
        {
            channelOptions.Credentials ??= scheme;
        };

        return channelOptions;
    }

    public void Dispose() => this._grpcChannel.Dispose();
}
