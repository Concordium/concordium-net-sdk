# concordium-net-sdk

[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](https://github.com/Concordium/.github/blob/main/.github/CODE_OF_CONDUCT.md)

Represents an API wirten on C# for interacting with the [Concordium Node](https://github.com/Concordium/concordium-node).

# ConcordiumNodeClient

The `ConcordiumNodeClient` is the main entrypoint for the SDK. It defines the api to be used to send and receive data from
the [Concordium Node](https://github.com/Concordium/concordium-node).

## Creating a client
The current `ConcordiumNodeClient` setup only allows for insecure connections, which can be set up in the following way.
The access is controlled by the [Connection](https://github.com/Concordium/concordium-node).
```csharp
Connection connection = new Connection
{
    Address = "http://localhost:10001",
    AuthenticationToken = "rpcadmin"
};
ConcordiumNodeClient concordiumNodeClient = new ConcordiumNodeClient(connection);
```

# API Overview

## Peer Connect
The `PeerConnectAsync` connects to a specified peer.
- `ip` - the IP of the peer we want to connect to.
- `port`- the port of the peer we want to connect to.

```csharp
// create a concordiumNodeClient

bool isConnected = await concordiumNodeClient.PeerConnectAsync("127.0.0.1", 10001);
```

## Peer Disconnect
The `PeerDisconnectAsync` disconnects from a specified peer.
- `ip` - the IP of the peer we want to disconnect from.
- `port`- the port of the peer we want to disconnect from.

```csharp
// create a concordiumNodeClient

bool isDisconnected = await concordiumNodeClient.PeerDisconnectAsync("127.0.0.1", 10001);
```

## Get Peer Uptime
The `GetPeerUptimeAsync` retrieves an information about a node uptime.

```csharp
// create a concordiumNodeClient

ulong peerUptime = await concordiumNodeClient.GetPeerUptimeAsync();
```

## Get Peer Total Sent
The `GetPeerTotalSentAsync` retrieves an information about a node total send count..

```csharp
// create a concordiumNodeClient

ulong peerTotalSent = await concordiumNodeClient.GetPeerTotalSentAsync();
```

## Get Peer Total Received
The `GetPeerTotalReceivedAsync` retrieves an information about a node total received count.

```csharp
// create a concordiumNodeClient

bool peerTotalReceived = await concordiumNodeClient.GetPeerTotalReceivedAsync();
```

## Get Peer Version
The `GetPeerVersionAsync` retrieves an information about a node version.

```csharp
// create a concordiumNodeClient

string peerVersion = await concordiumNodeClient.GetPeerVersionAsync();
```

## Get Peer Stats
The `GetPeerStatsAsync` retrieves an information about a node stats.
- `includeBootstrappers` - does include bootstrappers. By default is `false`.

```csharp
// create a concordiumNodeClient

PeerStatsResponse peerStats = await concordiumNodeClient.GetPeerStatsAsync();
```

## Get Peer List
The `GetPeerListAsync` retrieves an information about a node list.
- `includeBootstrappers` - does include bootstrappers. By default is `false`.

```csharp
// create a concordiumNodeClient

PeerListResponse peerList = await concordiumNodeClient.GetPeerListAsync();
```

## Ban Node
The `BanNodeAsync` ban a node. The node to ban can either be supplied via a node-id or via an IP and port, but not both.
- `ip` - the node IP.
- `nodeId`- the node ID.
- `catchupStatus`- the catchup status.
- `port`- the node port.

```csharp
// create a concordiumNodeClient

bool isBannedNode = await concordiumNodeClient.BanNodeAsync("127.0.0.1", null, PeerElement.Types.CatchupStatus.Pending, 1001);
```

## Unban Node
The `UnbanNodeAsync` unban a node. The node to unban can either be supplied via a node-id or via an IP.
- `ip` - the node IP.
- `nodeId`- the node ID.
- `catchupStatus`- the catchup status.
- `port`- the node port.

```csharp
// create a concordiumNodeClient

bool isUnbannedNode = await concordiumNodeClient.UnbanNodeAsync("127.0.0.1", null, PeerElement.Types.CatchupStatus.Pending, 1001);
```

## Shutdown
The `ShutdownAsync` shutdown the node gracefully.

```csharp
// create a concordiumNodeClient

bool isShutdowned = await concordiumNodeClient.ShutdownAsync();
```

## Dump Start
The `DumpStartAsync` start dumping the packages.
- `file` - the file.
- `isRaw`- does raw. The default is `false`.

```csharp
// create a concordiumNodeClient

bool isDumpStarted = await concordiumNodeClient.DumpStartAsync("some file data");
```

## Dump Stop
The `DumpStopAsync` stop dumping the packages.

```csharp
// create a concordiumNodeClient

bool isDumpStoped = await concordiumNodeClient.DumpStopAsync();
```

## Join Network
The `JoinNetworkAsync` join a network.
- `networkId` - the network id.

```csharp
// create a concordiumNodeClient

bool isNetworkJoined = await concordiumNodeClient.JoinNetworkAsync(103);
```

## Leave Network
The `LeaveNetworkAsync` leave a network.
- `networkId` - the network id.

```csharp
// create a concordiumNodeClient

bool isNetworkLeft = await concordiumNodeClient.LeaveNetworkAsync(103);
```

## Start Baker
The `StartBakerAsync` start the baker.

```csharp
// create a concordiumNodeClient

bool isBakerStarted = await concordiumNodeClient.StartBakerAsync();
```

## Stop Baker
The `StopBakerAsync` stop the baker.

```csharp
// create a concordiumNodeClient

bool isBakerStoped = await concordiumNodeClient.StopBakerAsync();
```

## Get Node Info
The `GetNodeInfoAsync` retrieves an information about a node.

```csharp
// create a concordiumNodeClient

NodeInfoResponse nodeInfo = await concordiumNodeClient.GetNodeInfoAsync();
```

## Get Account Info
The `GetAccountInfoAsync` retrieves an information about a state of account corresponding to account address and block hash.
- `accountAddress` - the base58 check with version byte 1 encoded address (with Bitcoin mapping table).
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

AccountAddress accountAddress = AccountAddress.From("32gxbDZj3aCr5RYnKJFkigPazHinKcnAhkxpade17htB4fj6DN");
BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

AccountInfo? accountInfo = await concordiumNodeClient.GetAccountInfoAsync(accountAddress, blockHash);
```

## Get Next Account Nonce
The `GetNextAccountNonceAsync` retrieves the best guess as to what the next account nonce should be. If all account transactions are finalized then this information is reliable. Otherwise this is the best guess, assuming all other transactions will be committed to blocks and eventually finalized.
- `accountAddress` - the base58 check with version byte 1 encoded address (with Bitcoin mapping table).

```csharp
// create a concordiumNodeClient

AccountAddress accountAddress = AccountAddress.From("32gxbDZj3aCr5RYnKJFkigPazHinKcnAhkxpade17htB4fj6DN");

NextAccountNonce? nextAccountNonce = await concordiumNodeClient.GetNextAccountNonceAsync(accountAddress);
```

## Get Reward Status
The `GetRewardStatusAsync` retrieves the information about a current balance of special accounts.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

RewardStatus? rewardStatus = await concordiumNodeClient.GetRewardStatusAsync(blockHash);
```

## Get Birk Parameters
The `GetBirkParametersAsync` retrieves the information about a parameters used for baking.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

BirkParameters? birkParameters = await concordiumNodeClient.GetBirkParametersAsync(blockHash);
```

## Get Module List
The `GetModuleListAsync` retrieves the list of smart contract modules.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

List<ModuleRef> moduleList = await concordiumNodeClient.GetModuleListAsync(blockHash);
```

## Get Identity Providers
The `GetIdentityProvidersAsync` retrieves a list of identity providers in a specific block.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

List<IdentityProviderInfo> identityProviders = await concordiumNodeClient.GetIdentityProvidersAsync(blockHash);
```

## Get Anonymity Revokers
The `GetAnonymityRevokersAsync` retrieves a list of anonymity revokers in a specific block.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

List<AnonymityRevokerInfo> anonymityRevokers = await concordiumNodeClient.GetAnonymityRevokersAsync(blockHash);
```

## Get Cryptographic Parameters
The `GetCryptographicParametersAsync` retrieves the information about a cryptographic parameters in a specific block.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

VersionedValue<CryptographicParameters>? cryptographicParameters = await concordiumNodeClient.GetCryptographicParametersAsync(blockHash);
```

## Get Banned Peers
The `GetBannedPeersAsync` retrieves the information about a banned peers.

```csharp
// create a concordiumNodeClient

PeerListResponse bannedPeers = await concordiumNodeClient.GetBannedPeersAsync();
```

## Get Consensus Status
The `GetConsensusStatusAsync` retrieves an information about a current state of the consensus layer.

```csharp
// create a concordiumNodeClient

ConsensusStatus? consensusStatus = await concordiumNodeClient.GetConsensusStatusAsync();
```

## Get Account List
The `GetAccountListAsync` retrieves a list of accounts that exist in the given block. Empty list indicates the block does not exist.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

List<AccountAddress> accountList = await concordiumNodeClient.GetAccountListAsync(blockHash);
```

## Get Block Info
The `GetBlockInfoAsync` retrieves an information about a particular block with various details.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

BlockInfo? blockInfo = await concordiumNodeClient.GetBlockInfoAsync(blockHash);
```

## Get Instances
The `GetInstancesAsync` retrieves a list of smart contract instances that exist in the given block. Empty list indicates that the block does not exist.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

List<ContractAddress> instances = await concordiumNodeClient.GetInstancesAsync(blockHash);
```

## Get Instance Info
The `GetInstanceInfoAsync` retrieves the information about the specific smart contract instance.
- `contractAddress`- the contract address.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

ContractAddress contractAddress = ContractAddress.Create(1, 1);
BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

ContractInfo? instanceInfo = await concordiumNodeClient.GetInstanceInfoAsync(blockHash);
```

## Get Ancestors
The `GetAncestorsAsync` returns the list of the given block hash and the hashes of its ancestors going back the given number of generations. The length of the list will be the given number, or the list will be the entire chain going back from the given block until the closest genesis or regenesis block. If the block is not live or finalized, the function returns null.
- `amount`- the number of hash and hashes of its ancestors generations.
- `blockHash`- the base16 encoded hash of a block (64 characters).

```csharp
// create a concordiumNodeClient

ulong amount = 1000;
BlockHash blockHash = BlockHash.From("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af");

List<BlockHash> ancestors = await concordiumNodeClient.GetAncestorsAsync(amount, blockHash);
```

## Get Branches
The `GetBranchesAsync` returns branches of the tree from the last finalized block.

```csharp
// create a concordiumNodeClient

Branch branches = await concordiumNodeClient.GetBranchesAsync();
```


## Get Blocks At Height
The `GetBlocksAtHeightAsync` returns all blocks at the given height.
- `blockHeight`- the height of the blocks to query.
- `fromGenesisIndex`- the base genesis index. The default is 0.
- `restrictToGenesisIndex`- does restrict to specified genesis index. The default is false.

```csharp
// create a concordiumNodeClient

ulong blockHeight = 10u;

List<BlockHash> blocksAtHeight = await concordiumNodeClient.GetBlocksAtHeightAsync(blockHeight);
```

## Decrypt encrypted sign key
`ISignKeyEncryption.Decrypt` decrypts encrypted sign key. 

```csharp
public Ed25519SignKey Decrypt(EncryptedSignKey encryptedSignKey);
```

**Input:**

- [EncryptedSignKey]() `encryptedSignKey`: encrypted sign key
    - [Password]() `password`: password
    - [EncryptedSignKeyMetadata]() `metadata`: encrypted sign key metadata
        - [Salt]() `salt`: salt
        - [InitializationVector]() `initializationVector`: initialization vector
        - [int]() `iterations`: iterations
        - [HashAlgorithmName]() `hashAlgorithmName`: hash algorithm name
        - [int]() `keySize`: key size
    - [CipherText]() `cipherText`: cipher text


**Output:**

- The [Ed25519SignKey]() object containing information about a hex encoded ed25519 sign key.

### Code Sample
```csharp
Salt salt = new Salt("QsY4+h31LMs974pPN6QfsA==");
InitializationVector initializationVector = new InitializationVector("kzyQ24xum3WibCKfvngMlg==");
int iterations = 100000;
HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
string password = "111111";
CipherText cipherText = new CipherText("9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE=");

EncryptedSignKeyMetadata encryptedSignKeyMetadata = new EncryptedSignKeyMetadata(
    salt,
    initializationVector,
    iterations,
    hashAlgorithmName);

EncryptedSignKey encryptedSignKey = new EncryptedSignKey(
    password,
    encryptedSignKeyMetadata,
    cipherText);

Ed25519SignKey signKey = new SignKeyEncryption().Decrypt(encryptedSignKey);
```
