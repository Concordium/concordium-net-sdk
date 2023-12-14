## Unreleased changes

## 4.2.1
- Bugfix
  - Fix wrong build of rust dependencies which made the interops call not work on iOS.

## 4.2.0
- Added
  - Deserialization from module schema
    - Contract entrypoint messages
    - Contract events
    - Module schema
  - New GRPC-endpoints: `GetBlocks`, `GetFinalizedBlocks`, `GetBranches`, `GetAncestors`, `GetBlockPendingUpdates`
  - Added helpers to get new type `ContractIdentifier` on `ReceiveName` and `ContractName`. This new type only holds the contract name part of `ReceiveName` and `ContractName`.
    Also added helper to get entrypoint on `ReceiveName`.

## 4.1.0
- Bugfix
  - Documentation was missing when adding library as nuget packages and hovering over methods and classes.
- Added
  - gRPC queries relevant for smart contracts.
    - GetModuleListAsync
    - GetInstanceListAsync
    - GetModuleSourceAsync
    - GetInstanceInfoAsync

## 4.0.1
- Bugfix
  - `TransactionCount` in `BlockInfo` had wrong mapping and used `TransactionsSize` from node.
  - `BakerPoolInfo` had `OpenStatus` hardcoded as `BakerPoolOpenStatus.OpenForAll`.

## 4.0.0
- The SDK requires node version 6 or later.
- Breaking changes
  - `ConsensusInfo`
    - `SlotDuration` is now a nullable field, only present in protocols 1-5.
    - Bugfix: `BlockLastArrivedTime` was wrongly mapped from `BlockLastReceivedTime`.    
  - `BlockInfo`
    - `BlockSlot` is now a nullable field, and only present in protocols 1-5
- Added
  - `ConsensusInfo`
    - a new field `ConcordiumBftDetails` is added that is present if protocol version is 6 or higher
  - `BlockInfo`
    - new fields `Round` and `Epoch` that are present in protocol 6 or higher.
  - `BakerPoolStatus` 
    - a new field`BakerPoolPendingChange` is added which is present if any change is pending on baker pool.

## 3.0.0
- Added
  - Add optional cancellation token parameter to all client calls.

- Obsolete
  - Made former constructors on `ConcordiumClient` and `RawClient` obsolete in favor of new overload which takes `ConcordiumClientOptions`. Using this makes it easier to used from configurations and extending with additional properties in the future.
- Breaking changes
  - Data structures have been aligned throughout the SDK which has resulted in some major changes. Changes are:
    - Records are used when structures need immutability and equality by value.
    - Struct are used when data structures is below 16 bytes, and when they are not used  through any interfaces.
    - Sealed are added where inheritance isn't expected.
    - Structs are either records (and hence as default override) or override equality
    - Record structs has readonly where immutability is expected.
    - Implicit operators are removed to favor being explicit.
  - Add ConfigureAwait to enhance library uses from UI- or own syncronization context   usages.
  - `RawClient` asynchronous calls have been changed to return either `AsyncUnaryCall<T>`   or `AsyncServerStreamingCall` such that response header is available in   `ConcordiumClient`. 
    One can get the response be calling `.ResponseAsync` on `AsyncUnaryCall<T>` and   `ResponseStream` on `AsyncServerStreamingCall`.
  - Property `Timeout` moved into `Options` in class `RawClient`. `Timeout` now defaults to 'indefinitely' compared to 30 seconds in obsolete constructor of `ConcordiumClient`.
  - `AccountTransactionType` is renamed to `TransactionType`.
  - Bugfix: Record `InvalidInitMethod` had parameter change from `ContractName` to `InitName`.
  - Bugfix: Renamed `MissingDelegationAddParameter` to `MissingDelegationAddParameters`.


## 2.0.0
- Rewrite the SDK to use the Concordium Node gRPC API version 2. This
  rewrite does not preserve any of the old API and therefore deprecates
  earlier versions of the SDK.

## 1.0.0
- Initial release.
