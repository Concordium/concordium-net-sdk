using System.Runtime.CompilerServices;
using Concordium.Grpc.V2;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Grpc.Core;
using Grpc.Net.Client;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;
using AccountInfo = Concordium.Sdk.Types.AccountInfo;
using BakerId = Concordium.Sdk.Types.BakerId;
using BlockHash = Concordium.Sdk.Types.BlockHash;
using BlockInfo = Concordium.Sdk.Types.BlockInfo;
using BlockItemSummary = Concordium.Sdk.Types.BlockItemSummary;
using ConsensusInfo = Concordium.Sdk.Types.ConsensusInfo;
using FinalizationSummary = Concordium.Sdk.Types.FinalizationSummary;
using IpInfo = Concordium.Sdk.Types.IpInfo;
using TransactionHash = Concordium.Sdk.Types.TransactionHash;

namespace Concordium.Sdk.Client;

/// <summary>
/// A client for interacting with the Concordium gRPC API V2 exposed by nodes.
/// </summary>
public sealed class ConcordiumClient : IDisposable
{
    /// <summary>
    /// The raw client.
    /// </summary>
    public RawClient Raw { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
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
    public ConcordiumClient(Uri endpoint, ushort port, ulong? timeout = 30, GrpcChannelOptions? rawChannelOptions = null)
    => this.Raw = new(endpoint, port, timeout, rawChannelOptions);

    /// <summary>
    /// Send an account transaction to the node.
    /// </summary>
    /// <param name="transaction">The account transaction to send.</param>
    /// <returns>Hash of the transaction if it was accepted by the node.</returns>
    /// <exception cref="RpcException">The call failed, e.g. due to the node not accepting the transaction.</exception>
    /// <exception cref="FormatException">The returned transaction hash has an invalid number of bytes.</exception>
    public TransactionHash SendAccountTransaction(SignedAccountTransaction transaction)
    {
        // Send the transaction as a block item request.
        var txHash = this.Raw.SendBlockItem(transaction.ToSendBlockItemRequest());

        // Return the transaction hash as a "native" SDK type.
        return TransactionHash.From(txHash.Value.ToByteArray());
    }

    /// <summary>
    /// Spawn a task which sends an account transaction to the node.
    ///
    /// Note that the task may throw a <see cref="FormatException"/> if the transaction hash it returns
    /// has an invalid number of bytes.
    /// </summary>
    /// <param name="transaction">The account transaction to send.</param>
    /// <returns>Task which returns the hash of the transaction if it was accepted by the node.</returns>
    /// <exception cref="RpcException">The call failed, e.g. due to the node not accepting the transaction.</exception>
    public async Task<TransactionHash> SendAccountTransactionAsync(
        SignedAccountTransaction transaction
    )
    {
        // Send the transaction as a block item request.
        var txHash = await this.Raw
            .SendBlockItemAsync(transaction.ToSendBlockItemRequest())
            .ConfigureAwait(false);

        // Return the "native" SDK type.
        return TransactionHash.From(txHash.Value.ToByteArray());
    }

    /// <summary>
    /// Query the chain for the next sequence number of the account with the supplied
    /// address.
    ///
    /// Specifically, this is the best guess as to what the next account sequence
    /// number (nonce) should be for the given account.
    ///
    /// If all account transactions are finalized then this information is reliable.
    ///
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    /// <param name="accountAddress">An address of the account to get the next sequence number of.</param>
    /// <returns>
    /// A tuple containing the best guess of the next sequence number for the supplied account and a boolean
    /// indicating whether all transactions associated with the account are finalized. If the latter is the
    /// case, then the sequence number is reliable.
    /// </returns>
    /// <exception cref="RpcException">The call failed.</exception>
    public (AccountSequenceNumber, bool) GetNextAccountSequenceNumber(
        AccountAddress accountAddress
    )
    {
        var next = this.Raw.GetNextAccountSequenceNumber(accountAddress.ToProto());

        // Return the sequence number as a "native" SDK type.
        return (AccountSequenceNumber.From(next.SequenceNumber.Value), next.AllFinal);
    }

    /// <summary>
    /// Spawn a task which queries the chain for the next sequence number of the
    /// account with the supplied address.
    ///
    /// Specifically, this is the best guess as to what the next account sequence
    /// number (nonce) should be for the given account.
    ///
    /// If all account transactions are finalized then this information is reliable.
    ///
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    /// <param name="accountAddress">An address of the account to get the next sequence number of.</param>
    /// <returns>
    /// A task which returns a tuple containing the best guess of the next sequence number for the supplied
    /// account and a boolean indicating whether all transactions associated with the account are finalized.
    /// If the latter is the case, then the sequence number is reliable.
    /// </returns>
    /// <exception cref="RpcException">The call failed.</exception>
    public async Task<(AccountSequenceNumber, bool)> GetNextAccountSequenceNumberAsync(
        AccountAddress accountAddress
    )
    {
        var next = await this.Raw
            .GetNextAccountSequenceNumberAsync(accountAddress.ToProto())
            .ConfigureAwait(false);

        return (AccountSequenceNumber.From(next.SequenceNumber.Value), next.AllFinal);
    }

    /// <summary>
    /// Get the status of and information about a specific block item given a transaction hash.
    /// </summary>
    /// <param name="transactionHash">Transaction Hash which is included in blocks returned.</param>
    /// <returns>A common return type for TransactionStatus.</returns>
    public ITransactionStatus GetBlockItemStatus(TransactionHash transactionHash)
    {
        var blockItemStatus = this.Raw.GetBlockItemStatus(transactionHash.ToProto());

        return TransactionStatusFactory.CreateTransactionStatus(blockItemStatus);
    }

    /// <summary>
    /// Get the status of and information about a specific block item given a transaction hash.
    /// </summary>
    /// <param name="transactionHash">Transaction Hash which is included in blocks returned.</param>
    /// <returns>Task which wraps a common return type for TransactionStatus.</returns>
    public async Task<ITransactionStatus> GetBlockItemStatusAsync(TransactionHash transactionHash)
    {
        var blockItemStatus = await this.Raw.GetBlockItemStatusAsync(transactionHash.ToProto())
            .ConfigureAwait(false);

        return TransactionStatusFactory.CreateTransactionStatus(blockItemStatus);
    }

    /// <summary>
    /// Get the information for the given account in the given block.
    /// </summary>
    /// <param name="accountAddress">An address of the account.</param>
    /// <param name="blockHash">A hash of the block</param>
    /// <returns>Account information at the given block.</returns>
    public async Task<AccountInfo> GetAccountInfoAsync(AccountAddress accountAddress, IBlockHashInput blockHash)
    {
        var accountInfoRequest = new AccountInfoRequest
        {
            BlockHash = blockHash.Into(),
            AccountIdentifier = accountAddress.ToAccountIdentifierInput()
        };
        var accountInfoAsync = await this.Raw.GetAccountInfoAsync(accountInfoRequest);
        return AccountInfo.From(accountInfoAsync);
    }

    /// <summary>
    /// Get the list of accounts in the given block.
    /// The stream will end when all accounts that exist in the state at the end
    /// of the given block have been returned.
    /// </summary>
    /// <param name="input">A hash of block</param>
    /// <returns>Accounts at given block</returns>
    /// <exception cref="RpcException">Block doesn't exists.</exception>
    public async IAsyncEnumerable<AccountAddress> GetAccountListAsync(IBlockHashInput input)
    {
        var blockHash = input.Into();
        await foreach (var account in this.Raw.GetAccountList(blockHash).ConfigureAwait(false))
        {
            yield return AccountAddress.From(account);
        }
    }

    /// <summary>
    /// Retrieve version of the software on the node.
    /// </summary>
    /// <returns>Version of the node in semantic format</returns>
    public async Task<PeerVersion> GetPeerVersionAsync()
    {
        var nodeInfo = await this.Raw.GetNodeInfoAsync();
        return PeerVersion.Parse(nodeInfo.PeerVersion);
    }

    /// <summary>
    /// Get information about a given pool at the end of a given block.
    /// If the block does not exist or is prior to protocol version 4 then
    /// exception is thrown.
    /// TODO: Old name GetPoolStatusForBakerAsync
    /// </summary>
    /// <param name="bakerId">Id of baker</param>
    /// <param name="blockHash">Block hash from where information will be fetched</param>
    /// <returns>The state of the baker currently registered on the account.</returns>
    public async Task<BakerPoolStatus> GetPoolInfoAsync(BakerId bakerId, IBlockHashInput blockHash)
    {
        var poolInfoAsync = await this.Raw.GetPoolInfoAsync(new PoolInfoRequest
        {
            BlockHash = blockHash.Into(),
            Baker = bakerId.ToProto()
        });
        return BakerPoolStatus.From(poolInfoAsync);
    }

    /// <summary>
    /// Get information about tokenomics at the end of a given block.
    /// If the block does not exist [`QueryError::NotFound`] is returned.
    /// TODO: Old GetRewardStatusAsync
    /// </summary>
    /// <param name="blockHash">Block from where state of tokenomics are returned.</param>
    /// <returns>Tokenomics</returns>
    /// <exception cref="RpcException">Block doesn't exists.</exception>
    public async Task<RewardOverviewBase> GetTokenomicsInfoAsync(IBlockHashInput blockHash)
    {
        var tokenomicsInfo = await this.Raw.GetTokenomicsInfoAsync(blockHash.Into());
        return RewardOverviewBase.From(tokenomicsInfo);
    }

    /// <summary>
    /// State of the passive delegation pool at block hash. Changes to delegation,
    /// e.g., an account deciding to delegate are reflected in this structure at
    /// first.
    /// TODO: Old GetPoolStatusForPassiveDelegation
    /// </summary>
    /// <param name="blockHash">Block hash from where passive delegation status will be returned.</param>
    /// <returns>State of the passive delegation pool</returns>
    public async Task<PassiveDelegationStatus> GetPassiveDelegationInfoAsync(IBlockHashInput blockHash)
    {
        var passiveDelegationInfoAsync = await this.Raw.GetPassiveDelegationInfoAsync(blockHash.Into());
        return PassiveDelegationStatus.From(passiveDelegationInfoAsync);
    }

    /// <summary>
    /// Get information about the current state of consensus. This is an
    /// overview of the node's current view of the chain.
    /// TODO: Old GetConsensusStatusAsync
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>Current consensus information.</returns>
    public async Task<ConsensusInfo> GetConsensusInfoAsync(CancellationToken token = default)
    {
        var consensusInfoAsync = await this.Raw.GetConsensusInfoAsync(token)
            .ConfigureAwait(false);
        return ConsensusInfo.From(consensusInfoAsync);
    }

    /// <summary>
    /// Get a list of live blocks at a given height.
    /// </summary>
    /// <param name="blockHeight">Block height from where blocks should be returned.</param>
    /// <param name="token">Cancellation Token.</param>
    /// <returns>List of live blocks.</returns>
    public async Task<IList<BlockHash>> GetBlocksAtHeightAsync(IBlockHeight blockHeight, CancellationToken token = default)
    {
        var blocksAtHeightResponse = await this.Raw.GetBlocksAtHeightAsync(blockHeight.Into(), token);

        return blocksAtHeightResponse.Blocks.Select(BlockHash.From).ToList();
    }

    /// <summary>
    /// Get information, such as height, timings, and transaction counts for the
    /// given block. If the block does not exist [`QueryError::NotFound`] is
    /// returned.
    /// </summary>
    /// <param name="blockHash">Block from where information will be returned.</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Block information from requested block.</returns>
    public async Task<BlockInfo> GetBlockInfoAsync(IBlockHashInput blockHash, CancellationToken token = default)
    {
        var blockInfo = await this.Raw.GetBlockInfoAsync(blockHash.Into(), token);
        return BlockInfo.From(blockInfo);
    }

    /// <summary>
    /// Get a the special events in a given block. The stream will end when all the
    /// special events for a given block have been returned.
    /// These are events generated by the protocol, such as minting and reward
    /// payouts. They are not directly generated by any transaction.
    /// </summary>
    /// <param name="blockHashInput"></param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Special events at given block</returns>
    /// <exception cref="RpcException">If the block does not exist</exception>
    public async IAsyncEnumerable<ISpecialEvent> GetBlockSpecialEvents(IBlockHashInput blockHashInput,[EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach(var specialEvent in this.Raw.GetBlockSpecialEvents(blockHashInput.Into(), token).ConfigureAwait(false))
        {
            yield return specialEvent.Into();
        }
    }

    /// <summary>
    /// Get the information about a finalization record in a block.
    /// A block can contain zero or one finalization record. If a record is
    /// contained then this query will return information about the finalization
    /// session that produced it, including the finalizers eligible for the
    /// session, their power, and whether they signed this particular record.
    /// If the block contains zero finalization record the finalization summary is null.
    /// If the block does not exist an exception is thrown is returned.
    /// </summary>
    /// <param name="blockHashInput">Block from where finalization summary will be given.</param>
    public async Task<FinalizationSummary?> GetBlockFinalizationSummaryAsync(IBlockHashInput blockHashInput)
    {
        var finalizationSummary = await this.Raw.GetBlockFinalizationSummaryAsync(blockHashInput.Into());
        return FinalizationSummary.From(finalizationSummary);
    }

    /// <summary>
    /// Get the transaction events in a given block. The stream will end when all the
    /// transaction events for a given block have been returned.
    /// </summary>
    /// <param name="blockHashInput">Block from where transactions events will be returned.</param>
    /// <returns>Summary of a block item with transactional meta data.</returns>
    /// <exception cref="RpcException">If the block does not exist</exception>
    public async IAsyncEnumerable<BlockItemSummary> GetBlockTransactionEvents(IBlockHashInput blockHashInput)
    {
        var blockTransactionEvents = this.Raw.GetBlockTransactionEvents(blockHashInput.Into());
        await foreach (var blockItemSummary in blockTransactionEvents)
        {
            yield return BlockItemSummary.From(blockItemSummary);
        }
    }

    /// <summary>
    /// Get the identity providers registered as of the end of a given block.
    /// The stream will end when all the identity providers have been returned.
    /// </summary>
    /// <param name="blockHash">Block hash from where identity providers state will be returned</param>
    /// <returns>Identity providers info</returns>
    /// <exception cref="RpcException">If the block does not exist</exception>
    public async Task<QueryResponse<IAsyncEnumerable<IpInfo>>> GetIdentityProvidersAsync(IBlockHashInput blockHash)
    {
        var response = await this.Raw.GetIdentityProviders(blockHash.Into());
        var returnEnumerable = response.Response.Select(IpInfo.From);
        return response.NewWithSameBlockHash(returnEnumerable);
    }

    /// <summary>
    /// Get all the bakers at the end of the given block.
    /// </summary>
    /// <param name="blockHash">Block hash from where state of baker list will be returned.</param>
    /// <returns>List of baker ids.</returns>
    /// <exception cref="RpcException">If the block does not exist</exception>
    public async Task<QueryResponse<IAsyncEnumerable<BakerId>>> GetBakerListAsync(IBlockHashInput blockHash)
    {
        var response = await this.Raw.GetBakerList(blockHash.Into());
        var returnEnumerable = response.Response.Select(BakerId.From);
        return response.NewWithSameBlockHash(returnEnumerable);
    }

    /// <summary>
    /// Get the chain parameters in effect after a given block.
    /// </summary>
    /// <param name="blockHash">Block hash from where chain parameters will be given.</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Chain parameters at given block.</returns>
    /// <exception cref="RpcException">If the block does not exist</exception>
    public async Task<QueryResponse<IChainParameters>> GetBlockChainParametersAsync(IBlockHashInput blockHash, CancellationToken token = default)
    {
        var response = await this.Raw.GetBlockChainParametersAsync(blockHash.Into(), token);
        var chainParameters = ChainParametersFactory.From(response.Response);
        return response.NewWithSameBlockHash(chainParameters);
    }

    public void Dispose() => this.Raw.Dispose();
}
