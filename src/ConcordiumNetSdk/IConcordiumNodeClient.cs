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
using AccountAddress = ConcordiumNetSdk.Types.AccountAddress;
using BlockHash = ConcordiumNetSdk.Types.BlockHash;

namespace ConcordiumNetSdk;

public interface IConcordiumNodeClient
{
    /// <summary>
    /// Connects to a specified peer.
    /// </summary>
    /// <param name="ip">the IP of the peer we want to connect to.</param>
    /// <param name="port">the port of the peer we want to connect to.</param>
    /// <returns><see cref="Boolean"/> - the result of connecting to a specified peer.</returns>
    Task<bool> PeerConnectAsync(string ip, int? port = null);

    /// <summary>
    /// Disconnects from a specified peer.
    /// </summary>
    /// <param name="ip">the IP of the peer we want to disconnect from.</param>
    /// <param name="port">the port of the peer we want to disconnect from.</param>
    /// <returns><see cref="Boolean"/> - the result of disconnecting to a specified peer.</returns>
    Task<bool> PeerDisconnectAsync(string ip, int? port = null);

    /// <summary>
    /// Retrieves an information about a node uptime.
    /// </summary>
    /// <returns><see cref="UInt64"/> - the node uptime.</returns>
    Task<ulong> GetPeerUptimeAsync();

    //todo: find more about documentation
    /// <summary>
    /// Retrieves an information about a node total send count.
    /// </summary>
    /// <returns><see cref="UInt64"/> - the node total send count.</returns>
    Task<ulong> GetPeerTotalSentAsync();

    //todo: find more about documentation
    /// <summary>
    /// Retrieves an information about a node total received count.
    /// </summary>
    /// <returns><see cref="UInt64"/> - the node total received count.</returns>
    Task<ulong> GetPeerTotalReceivedAsync();

    //todo: find more about documentation
    /// <summary>
    /// Retrieves an information about a node version.
    /// </summary>
    /// <returns><see cref="String"/> - the node version.</returns>
    Task<string> GetPeerVersionAsync();

    //todo: find more about documentation
    /// <summary>
    /// Retrieves an information about a node stats.
    /// </summary>
    /// <param name="includeBootstrappers">does include bootstrappers.</param>
    /// <returns><see cref="PeerStatsResponse"/> - the node stats.</returns>
    Task<PeerStatsResponse> GetPeerStatsAsync(bool includeBootstrappers = false);

    /// <summary>
    /// Retrieves an information about a state of account corresponding to account address and block hash.
    /// </summary>
    /// <param name="accountAddress">the base58 check with version byte 1 encoded address (with Bitcoin mapping table).</param>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="AccountInfo"/> - the state of an account in the given block.</returns>
    Task<AccountInfo?> GetAccountInfoAsync(AccountAddress accountAddress, BlockHash blockHash);

    /// <summary>
    /// Retrieves the best guess as to what the next account nonce should be.
    /// If all account transactions are finalized then this information is reliable.
    /// Otherwise this is the best guess, assuming all other transactions will be committed to blocks and eventually finalized.
    /// </summary>
    /// <param name="accountAddress">the base58 check with version byte 1 encoded address (with Bitcoin mapping table).</param>
    /// <returns><see cref="NextAccountNonce"/> - the next account nonce.</returns>
    Task<NextAccountNonce?> GetNextAccountNonceAsync(AccountAddress accountAddress);

    /// <summary>
    /// Retrieves the information about a current balance of special accounts.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="RewardStatus"/> - information about a current balance of special accounts.</returns>
    Task<RewardStatus?> GetRewardStatusAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves the information about a parameters used for baking.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="BirkParameters"/> - information about a parameters used for baking.</returns>
    Task<BirkParameters?> GetBirkParametersAsync(BlockHash blockHash);

    //todo: find out why List T is showing instead of real type
    /// <summary>
    /// Retrieves the list of smart contract modules.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="List{ModuleRef}"/> - list of smart contract modules.</returns>
    Task<List<ModuleRef>> GetModuleListAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves the source of the module as it was deployed on the chain.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <param name="moduleRef">the base16 encoded hash of a module ref (64 characters).</param>
    /// <returns><see cref="ByteString"/> - source of the module as it was deployed on the chain.</returns>
    Task<ByteString> GetModuleSourceAsync(BlockHash blockHash, ModuleRef moduleRef);

    /// <summary>
    /// Retrieves a list of identity providers in a specific block.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="List{IdentityProviderInfo}"/> - list of identity providers.</returns>
    Task<List<IdentityProviderInfo>> GetIdentityProvidersAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves a list of anonymity revokers in a specific block.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="List{AnonymityRevokerInfo}"/> - list of anonymity revokers.</returns>
    Task<List<AnonymityRevokerInfo>> GetAnonymityRevokersAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves the information about a cryptographic parameters in a specific block.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="CryptographicParameters"/> - information about a cryptographic parameters in a specific block.</returns>
    Task<VersionedValue<CryptographicParameters>?> GetCryptographicParametersAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves the information about a banned peers.
    /// </summary>
    /// <returns><see cref="PeerListResponse"/> - information about a banned peers.</returns>
    Task<PeerListResponse> GetBannedPeersAsync();

    /// <summary>
    /// Retrieves an information about a current state of the consensus layer.
    /// </summary>
    /// <returns><see cref="ConsensusStatus"/> - the consensus status.</returns>
    Task<ConsensusStatus?> GetConsensusStatusAsync();

    /// <summary>
    /// Retrieves a list of accounts that exist in the given block.
    /// Empty list indicates the block does not exist.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="List{AccountAddress}"/> - list of accounts that exist in the given block.</returns>
    Task<List<AccountAddress>> GetAccountListAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves an information about a particular block with various details.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="BlockInfo"/> - details about a particular block.</returns>
    Task<BlockInfo?> GetBlockInfoAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves a list of smart contract instances that exist in the given block.
    /// Empty list indicates that the block does not exist.
    /// </summary>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="List{ContractAddress}"/> - a list of contract instances.</returns>
    Task<List<ContractAddress>> GetInstancesAsync(BlockHash blockHash);

    /// <summary>
    /// Retrieves the information about the specific smart contract instance.
    /// </summary>
    /// <param name="contractAddress">the contract address.</param>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="ContractInfo"/> - the information about the specific smart contract instance.</returns>
    Task<ContractInfo?> GetInstanceInfoAsync(ContractAddress contractAddress, BlockHash blockHash);

    /// <summary>
    /// Returns the list of the given block hash and the hashes of its ancestors going back the given number of generations.
    /// The length of the list will be the given number, or the list will be the entire chain going back from the given block
    /// until the closest genesis or regenesis block.
    /// If the block is not live or finalized, the function returns null.
    /// </summary>
    /// <param name="amount">the number of hash and hashes of its ancestors generations.</param>
    /// <param name="blockHash">the base16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="List{BlockHash}"/> - the list of the given block hash and the hashes of its ancestors.</returns>
    Task<List<BlockHash>> GetAncestorsAsync(ulong amount, BlockHash blockHash);

    /// <summary>
    /// Returns branches of the tree from the last finalized block.
    /// </summary>
    /// <returns><see cref="Branch"/> - branches of the tree from the last finalized block.</returns>
    Task<Branch> GetBranchesAsync();

    /// <summary>
    /// Returns all blocks at the given height.
    /// </summary>
    /// <param name="blockHeight">the height of the blocks to query.</param>
    /// <param name="fromGenesisIndex">the base genesis index.</param>
    /// <param name="restrictToGenesisIndex">does restrict to specified genesis index.</param>
    /// <returns><see cref="List{BlockHash}"/> - the list of the blocks.</returns>
    Task<List<BlockHash>> GetBlocksAtHeightAsync(
        ulong blockHeight,
        uint fromGenesisIndex = 0,
        bool restrictToGenesisIndex = false);

    /// <summary>
    /// Sends any account transaction.
    /// </summary>
    /// <param name="payload">the binary encoding of the transaction (details <a href="https://github.com/Concordium/concordium-node/blob/main/docs/grpc-for-smart-contracts.md#sendtransaction">here</a>).</param>
    /// <param name="networkId">the id for the network. Default id is 100.</param>
    /// <returns><see cref="bool"/> - true or false depending on if transaction was successfully sent.</returns>
    Task<bool> SendTransactionAsync(byte[] payload, uint networkId = 100);
}
