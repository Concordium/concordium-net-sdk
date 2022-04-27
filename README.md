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

## Get account info
`GetAccountInfoAsync` retrieves an information about a state of account corresponding to `accountAddress` and `blockHash`. 

```csharp
public async Task<AccountInfo?> GetAccountInfoAsync(AccountAddress accountAddress, BlockHash blockHash);
```

**Input:**

- [AccountAddress]() `accountAddress`: base-58 check with version byte 1 encoded address (with Bitcoin mapping table).
- [BlockHash]() `blockHash`: base-16 encoded hash of a block (64 characters).


**Output:**

- The [AccountInfo](https://www.nuget.org/packages/StyleCop.Analyzers/) object containing information about a state of account. Returns `null` if the account was not found.

### Code Sample
```csharp
AccountAddress accountAddress = new AccountAddress("3sAHwfehRNEnXk28W7A3XB3GzyBiuQkXLNRmDwDGPUe8JsoAcU");
BlockHash blockHash = new BlockHash("6b01f2043d5621192480f4223644ef659dd5cda1e54a78fc64ad642587c73def");
AccountInfo accountInfo = await client.GetAccountInfoAsync(accountAddress, blockHash);
```
