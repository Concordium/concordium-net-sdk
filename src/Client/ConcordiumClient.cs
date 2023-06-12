using Concordium.Grpc.V2;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Concordium.Sdk.Types.Mapped;
using Concordium.Sdk.Types.New;
using Grpc.Core;
using Grpc.Net.Client;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;
using AccountInfo = Concordium.Sdk.Types.Mapped.AccountInfo;
using BlockHash = Concordium.Sdk.Types.BlockHash;
using BlockInfo = Concordium.Sdk.Types.New.BlockInfo;
using ConsensusInfo = Concordium.Sdk.Types.Mapped.ConsensusInfo;
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
            .SendBlockItemAsync(transaction.ToSendBlockItemRequest());

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
            .GetNextAccountSequenceNumberAsync(accountAddress.ToProto());

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
    public async Task<AccountInfo> GetAccountInfoAsync(AccountAddress accountAddress, BlockHash blockHash)
    {
        var accountInfoRequest = new AccountInfoRequest
        {
            BlockHash = blockHash.ToBlockHashInput(),
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
    public async IAsyncEnumerable<AccountAddress> GetAccountListAsync(BlockHash input)
    {
        var blockHash = input.ToBlockHashInput();
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
    public async Task<BakerPoolStatus> GetPoolInfoAsync(ulong bakerId, BlockHash blockHash)
    {
        var poolInfoAsync = await this.Raw.GetPoolInfoAsync(new PoolInfoRequest
        {
            BlockHash = blockHash.ToBlockHashInput(),
            Baker = new Grpc.V2.BakerId
            {
                Value = bakerId
            }
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
    /// /// <exception cref="RpcException">Block doesn't exists.</exception>
    public async Task<RewardOverviewBase> GetTokenomicsInfoAsync(BlockHash blockHash)
    {
        var tokenomicsInfo = await this.Raw.GetTokenomicsInfoAsync(blockHash.ToBlockHashInput());
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
    public async Task<PassiveDelegationStatus> GetPassiveDelegationInfoAsync(BlockHash blockHash)
    {
        var passiveDelegationInfoAsync = await this.Raw.GetPassiveDelegationInfoAsync(blockHash.ToBlockHashInput());
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
    public async Task<IList<BlockHash>> GetBlocksAtHeightAsync(IBlockHeight blockHeight, CancellationToken token)
    {
        var blocksAtHeightResponse = await this.Raw.GetBlocksAtHeightAsync(blockHeight.IntoRequest());

        return blocksAtHeightResponse.Blocks.Select(BlockHash.From).ToList();
    }

    public async Task<BlockInfo> GetBlockInfoAsync(BlockHash blockHash)
    {
        var blockInfo = await this.Raw.GetBlockInfoAsync(blockHash.ToBlockHashInput());
        return BlockInfo.From(blockInfo);
    }

    public async Task<BlockSummaryBase> GetBlockSummaryAsync(BlockHash blockHash)
    {
        var blockHashInput = blockHash.ToBlockHashInput();
        var events = this.Raw.GetBlockSpecialEvents(blockHashInput);
        var finalization = await this.Raw.GetBlockFinalizationSummaryAsync(blockHashInput);
        var blockTransactionEvents = this.Raw.GetBlockTransactionEvents(blockHashInput);
        return BlockSummaryBase.From(events, finalization, blockTransactionEvents);
    }

    public ValueTask<IdentityProviderInfo[]> GetIdentityProvidersAsync(BlockHash blockHash)
    {
        var identityProviders = this.Raw.GetIdentityProviders(blockHash.ToBlockHashInput());
        return IdentityProviderInfo.From(identityProviders);
    }

    public IAsyncEnumerable<ulong> GetBakerListAsync(BlockHash blockHash) =>
        this.Raw.GetBakerList(blockHash.ToBlockHashInput())
            .Select(b => b.Value);

    public void Dispose() => this.Raw.Dispose();
}
