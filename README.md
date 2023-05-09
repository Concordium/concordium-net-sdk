![CI](https://github.com/Concordium/concordium-net-sdk/actions/workflows/build-and-test.yaml/badge.svg)
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](https://github.com/Concordium/.github/blob/main/.github/CODE_OF_CONDUCT.md)
[![NuGet version](https://badge.fury.io/nu/ConcordiumNetSdk.svg)](https://badge.fury.io/nu/ConcordiumNetSdk)


# A .NET C# SDK for interacting with the Concordium blockchain

This SDK is a .NET integration library which adds support for constructing and sending various transactions, as well as querying various aspects of the blockchain and its nodes. The SDK uses version 2 of the [Concordium GRPC API](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) to interact with Concordium nodes and in turn the Concordium blockchain, and serves as a wrapper for this API with added ergonomics. Note that this completely deprecates earlier versions of the SDK that use version 1 of the API, cfr. the [migration](#migration) section for more details.

## Overview

This SDK is currently still under development and serves as a wrapper for the Concordium Node GRPC API V2 with helpers for common tasks.

Implementation-wise, this is first and foremost accomplished by exposing "minimal" wrappers for classes generated directly from the protocol buffer definitions of the [Concordium GRPC API](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) using the [`Grpc.Tools`](https://www.nuget.org/packages/Grpc.Tools/) and [`Grpc.Net.Client`](https://www.nuget.org/packages/Grpc.Net.Client) packages. This generation step results in a "raw" client class which exposes a method corresponding to each service definition in the protocol buffer definition as well as a class corresponding to each type declared in the API. See [Using the raw client API](#using-the-raw-client-api) for more information.

The wrappers are minimal in the sense that they are identical to those of the generated classes but devoid from the complexity of having to specify parameters for connection handling with each call. A drawback of this approach is that the generated classes are devoid of checks for any semantic invariants, which leaves much to be desired.

To remedy this, the SDK provides its own class equivalents corresponding to the most common raw API types, as well as wrappers for a subset of the raw service methods specifying these class equivalents at their interfaces instead. These transparently map input and output types to and from their native equivalents as they are marshalled across the underlying raw API, which allows for enforcing the invariants of the input and output data. Similarly, the SDK aims to provide functionality for working with and signing account transactions, as well as for importing keys and implementing custom signing logic. The latter can be useful e.g. when delegating signing to a HSM. See [Working with transaction signers](#working-with-transaction-signers) more information.

Currently, helpers for working with transactions of the [`Transfer`](), [`TransferWithMemo`]() and [`RegisterData`]() kind are provided. Ergonomic APIs that use the aforementioned native class equivalents of the raw API types at their interfaces are provided for [`GetNextAccountSequenceNumber`](..), [`GetNextAccountSequenceNumber`](..) and [`SendBlockItem`](). For more information on how to create and submit transactions, respectively using the ergonomic APIs, see the [Working with account transactions](#working-with-account-transactions) section, respectively [Using the client API](#using-the-client-api). Further transactions and wrappers are implemented on a per-need basis.

## Prerequisites/compatibility

- .NET Framework: 6.0 or later.
- Concordium Node version compatibility: 5.*

## Installation

The SDK is published on [nuget.org](https://www.nuget.org/packages/ConcordiumNetSdk), and depending on your setup, it can be added to your project as a dependency by running either

```powershell
PM> Install-Package Concordium.SDK -Version ???
```
or

```sh
dotnet add package Concordium.SDK
```
in your project root. It can also be used as a GIT submodule by embedding the cloned [repository](https://github.com/Concordium/concordium-net-sdk) directly into your project:
```
git clone https://github.com/Concordium/concordium-net-sdk --recurse-submodules
```

## Basic usage

At the core of the SDK is the [`ConcordiumClient`](...) class which is used to connect to a Concordium node and exposes methods interacting with it. The [`ConcordiumClient`](..) can be instantiated as follows:

```csharp
using Concordium.Sdk.Client;

// Construct the client.
ConcordiumClient client = new ConcordiumClient(
  new Uri("https://localhost/), // Endpoint  URL.
  options.Port, // Port.
  60 // Use a timeout of 60 seconds.
);
```

### Working with account transactions

Account transactions are blockchain transactions that are signed by and submitted on the behalf of an account. Classes pertaining to account transactions live in the `Concordium.SDK.Transactions` namespace. All account transactions are modeled by records that inherit from the abstract record [`AccountTransactionPayload<T>`](...), where `T` is the type of the transaction. Inheriting records contain data specific to the transaction. One example of such is the [`Transfer`](...) record representing the transfer of a CCD amount from one account to another. It is instantiated as follows:

```csharp
using Concordium.Sdk.Types;
using Concordium.Sdk.Transactions;

CcdAmount amount = CcdAmount.FromCcd(100); // Send 100 CCD.
AccountAddress receiver = AccountAddress.From("4rvQePs6ZKFiW8rwY5nP18Uj2DroWiw9VPKTsTwfwmsjcFCJLy");
Transfer transfer = new Transfer(amount, receiver);
```

Since an account transaction is to be submitted on behalf of an account any [`AccountTransactionPayload<T>`](..) must be annotated with the following before signing it: An [`AccountAddress`](..) of the sending account, a [`SequenceNumber`](..) (nonce) which is used to mitigate replay attacks and an [`Expiry`](...) representing a point in time after which the transaction will not be included in blocks whose (slot) time lies beyond it. The result of the annotation is a [`PreparedAccountTransaction<T>`](..):

```csharp
AccountAddress sender = AccountAddress.From("3jfAuU1c4kPE6GkpfYw4KcgvJngkgpFrD9SkDBgFW3aHmVB5r1");
SequenceNumber sequenceNumber = Client.GetNextAccountSequenceNumber(sender);
Expiry expiry = Expiry.AtMinutesFromNow(10); // Transaction should expire 10 minutes after current system time.
PreparedAccountTransaction<Transfer> preparedTransfer = transfer.Prepare(sender, sequenceNumber, expiry);
```

Finally, the transaction must be signed using an [`ITransactionSigner`](..) which implements signing with the (secret) sign keys of the sender account, producing a [`SignedAccountTransaction<Transfer>`](..). In the following example the implementation used is the [`WalletAccount`](..) class which supports importing of account keys from the Concordium browser-wallet key export format:

```csharp
ITransactionSigner signer = WalletAccount.FromWalletKeyExportFormat("/path/to/exported-browser-wallet-keys.json");
Expiry expiry = Expiry.AtMinutesFromNow(10); // Transaction expires 10 minutes after the current system time.
SignedAccountTransaction<Transfer> signedTransfer = preparedTransfer.Sign(signer);
```

A signed transaction can be submitted to the blockchain by invoking the [`SendTransaction`](..) method. If the transfer was accepted by the node, its [`TransactionHash`](..) used to uniquely identify the transaction is returned: 

```csharp
TransactionHash = client.SendTransaction(signedTransfer);
```

The [`TransactionHash`](..) can subsequently be used for querying the status of the transaction by invoking the "raw" [`GetBlockItemStatus`](..) method of the [`RawClient`](..). The instance of the `RawClient` can be accessed by `client.Raw` in the above example. For more info on raw API calls, see the [Raw client API](#raw-client-api) section.


### Working with transaction signers

As described in the [Account transactions](#account-transactions) section, account transactions must be signed using implementations of [`ITransactionSigner`](...). The SDK ships with the [`TransactionSigner`](..) class which is dictionary based implementation to which [`ISigner`](..)s can be added. An [`ISigner`](..) represents a concrete implementation of a (secret) sign key for an account, and can be used to to write custom signer implementations which can be useful, for instance, for delegating the signing logic to a HSM. The SDK also ships with the [`WalletAccount`](..) class which provides functionality for importing account keys from one of the supported wallet export formats. Note that this class furthermore holds the account address. Currently the Concordium browser and genesis wallet key export (JSON) formats are supported.

### Using the client API

As explained in the [overview](#overview) a subset of the raw methods generated from the Concordium Node GRPC API protocol buffer definitions have corresponding ergonomic wrappers that transparently map input and output types to and from their native equivalents as they are marshalled across the underlying generated API they wrap. One example of such is [`GetNextAccountSequenceNumber`](..) which takes an [`AccountAddress`](..) and returns a [`SequenceNumber`](..):

```csharp
AccountAddress sender = AccountAddress.From("3jfAuU1c4kPE6GkpfYw4KcgvJngkgpFrD9SkDBgFW3aHmVB5r1");
SequenceNumber sequenceNumber = client.GetNextAccountSequenceNumber(sender);
```

### Using the raw client API

As explained in the [overview](#overview), the entire [Concordium Node GRPC API V2](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) is exposed through minimal wrappers of classes that model the interface types and services as they were generated from the protocol buffer schema definitions using the [`Grpc.Net.Client`](..) and [`Grpc.Tools`](..) packages. These wrappers are defined in the [`RawClient`](..), instances of which can *only* be accessed through the `Raw` field of [`ConcordiumClient`](..) instances. 

As an example, the raw API call `GetAccountInfo` defined in the [Concordium Node GRPC API V2](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) can be invoked through [`RawClient.GetAccountInfo`](..) by supplying an instance of its corresponding generated type for [`AccountInfoRequest`](https://developer.concordium.software/concordium-grpc-api/#concordium.v2.AccountInfoRequest). In the following, we wish to retrieve the information of an account in the last finalized block:

```csharp
using Concordium.Grpc.V2;

BlockHashInput blockHashInput = new BlockHashInput() { LastFinal = new Empty() };

// Construct the input for the raw API.
AccountInfoRequest request = new AccountInfoRequest
{
  BlockHash = blockHashInput,
  /// Instantiate and convert Concordium.Sdk.Types.AccountAddress
  /// to an AccountIdentifierInput which is needed for the
  /// AccountInfoRequest.
  AccountIdentifier = Concordium.Sdk.Types.AccountAddress
    .From("4rvQePs6ZKFiW8rwY5nP18Uj2DroWiw9VPKTsTwfwmsjcFCJLy")
    .ToAccountIdentifierInput()
};

AccountInfo accountInfo = client.Raw.GetAccountInfo(request);
```

Note that all generated types live in the `Concordium.Grpc.V2` namespace, and that there is a vast overlap between the names generated from the protocol buffer definitions file and the types declared in the SDK `Concordium.Types`. In the above we thus need to specify the intended namespace for [`AccountAddress`](..) to resolve ambiguity. Furthermore we leverage the convenience method [`ToAccountIdentifierInput`](..) of [`Concordium.SDK.AccountAddress`](..) to convert the the base58 address into its corresponding raw format. Also note that the overall structure of the types are one-to-one with the [Concordium GRPC API V2](https://github.com/Concordium/concordium-grpc-api/tree/main/v2/concordium), so we refer to its documentation and the [Runnable examples](#runnable-examples) section for more information on working with the raw API.


## Runnable examples

A number of runnable examples illustrating usage of the SDK API are contained in the [examples](./examples) directory. See

- [examples/RawClient](./examples/RawClient) demonstrate usage of the raw API methods exposed in `RawClient` (instances of which are accessible through the `Raw` field of the `ConcordiumClient`).
- [examples/Transactions](./examples/Transactions) demonstrate how to work with the various transaction types.

For instance, a runnable example similar to that given in [Working with account transactions](#working-with-account-transactions) is found in [examples/Transactions/Transfer](./examples/Transactions/Transfer). Compiling the project and running the resulting binary will print the following help message:

```
Concordium.Sdk.Examples.Transactions.Transfer 1.0.0
Copyright (C) 2023 Concordium.Sdk.Examples.Transactions.Transfer

ERROR(S):
  Required option 'a, amount' is missing.
  Required option 'r, receiver' is missing.
  Required option 'k, keys' is missing.

  -a, --amount      Required. Amount of CCD to transfer.

  -r, --receiver    Required. Receiver of the CCD to transfer.

  -k, --keys        Required. Path to a file with contents that is in the Concordium
                    browser wallet key export format.

  -e, --endpoint    (Default: https://localhost/) URL representing the endpoint where the
                    GRPC V2 API is served.

  -p, --port        (Default: 20000) Port at the endpoint where the GRPC V2 API is served.

  --help            Display this help screen.

  --version         Display version information.
```

To run the example with similar values, we invoke the binary as follows:

```
Concordium.Sdk.Examples.Transactions.Transfer -a 100 -r 4rvQePs6ZKFiW8rwY5nP18Uj2DroWiw9VPKTsTwfwmsjcFCJLy -k /path/to/exported-browser-wallet-keys.json
```

Here, the sender account address is contained in the file specified by the `--keys` option, and the expiration time defaults to 60 seconds, hence is why it is not specified. Upon successful submission of the transaction, the example program will print something like:

```
Successfully submitted transfer transaction with hash 6bc9bfac5ef4aa1988ab8b1ab6007a736d4b3fe7e52b942d69a030319d979f13
```


## Documentation

The rendered documentation is available at [https://pages.github.com/???](https://pages.github.com/???)

## Migration

This deprecates earlier versions of the Concordium .NET SDK that used version 1 of the Concordium Node GRPC API. In terms of semantics and the information carried in messages, the APIs are quite similar. In some cases endpoints in the version 1 API were split into several endpoints in the version 2 API to increase the granularity. A major difference between this and the previous version of the SDK is that this version currently does not support deploying contract modules with schemas.