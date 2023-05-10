![CI](https://github.com/Concordium/concordium-net-sdk/actions/workflows/dotnet.yml/badge.svg)
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](https://github.com/Concordium/.github/blob/main/.github/CODE_OF_CONDUCT.md)
[![NuGet version](https://badge.fury.io/nu/ConcordiumNetSdk.svg)](https://badge.fury.io/nu/ConcordiumNetSdk)


# C# .NET SDK for interacting with the Concordium blockchain

This is a .NET integration library written in C# which adds support for constructing and sending various transactions, as well as querying various aspects of the Concordium blockchain and its nodes. This SDK uses version 2 of the [Concordium Node gRPC API](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) to interact with Concordium nodes and in turn the Concordium blockchain, and serves as a wrapper for this API with added ergonomics. Note that this deprecates earlier versions of the SDK that use version 1 of the API, cfr. the [migration](#migration) section for more details.

Read ahead for a brief overview and some examples, or skip directly the [rendered documentation](#documentation). 

## Overview

This SDK is currently under development and serves as a wrapper for the [Concordium gRPC API](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) with helpers for common tasks.

Implementation-wise, this is first and foremost accomplished by exposing "minimal" wrappers for classes generated directly from the protocol buffer definitions of the API using the [`Grpc.Tools`](https://www.nuget.org/packages/Grpc.Tools/) and [`Grpc.Net.Client`](https://www.nuget.org/packages/Grpc.Net.Client) packages. This generation step results in a "raw" client class which exposes a method corresponding to each service definition in the protocol buffer definition as well as a class corresponding to each type declared in the API. These are used at the interface of the raw client class. The wrappers are minimal in the sense that they are identical to those of the generated classes but devoid from the complexity of having to specify parameters for connection handling with each call. A drawback of this approach is that the generated classes are devoid of checks for any semantic invariants, which leaves much to be desired. See [Using the raw client API](#using-the-raw-client-api) for more information.

To remedy this, the SDK provides its own class equivalents corresponding to the most common raw API types, as well as wrappers for a subset of the raw service methods specifying these class equivalents at their interfaces instead. These transparently map input and output types to and from their native equivalents as they are marshalled across the underlying raw API, enforcing the invariants of the input and output data. Similarly, the SDK provides functionality for working with and signing account transactions, as well as for importing keys and implementing signing logic. The latter can be useful e.g. when delegating signing to a HSM. See [Working with transaction signers](#working-with-transaction-signers) for more information.

Currently, helpers for working with transactions of the [`Transfer`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.Transfer.html), [`TransferWithMemo`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.TransferWithMemo.html) and [`RegisterData`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.RegisterData.html) kind are provided. Ergonomic APIs that use the aforementioned native class equivalents of the raw API types at their interfaces are provided for [`GetNextAccountSequenceNumber`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html#Concordium_Sdk_Client_ConcordiumClient_GetNextAccountSequenceNumber_Concordium_Sdk_Types_AccountAddress_) and [`SendAccountTransaction`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html#Concordium_Sdk_Client_ConcordiumClient_SendAccountTransaction__1_Concordium_Sdk_Transactions_SignedAccountTransaction___0__). For more information on how to create and submit transactions, respectively using the ergonomic APIs, see the [Working with account transactions](#working-with-account-transactions) section, respectively [Using the client API](#using-the-client-api). Further transactions and wrappers are implemented on a per-need basis.

## Prerequisites/compatibility

- .NET Framework: 6.0 or later.
- Concordium Node version compatibility: 5.*

## Installation

The SDK is published on [nuget.org](https://www.nuget.org/packages/ConcordiumNetSdk). Depending on your setup, it can be added to your project as a dependency by running either

```powershell
PM> Install-Package Concordium.SDK -Version ???
```
or

```sh
$ dotnet add package Concordium.SDK
```
in your project root. It can also be used as a GIT submodule by embedding the cloned [repository](https://github.com/Concordium/concordium-net-sdk) directly into your project:
```sh
$ git clone https://github.com/Concordium/concordium-net-sdk --recurse-submodules
```

## Basic usage

At the core of the SDK is the [`ConcordiumClient`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html) class which is used to connect to a Concordium node and exposes methods interacting with it. The [`ConcordiumClient`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html) can be instantiated as follows:

```csharp
using Concordium.Sdk.Client;

// Construct the client.
ConcordiumClient client = new ConcordiumClient(
  new Uri("https://localhost/), // Endpoint  URL.
  options.Port, // Port.
  60 // Use a connection timeout of 60 seconds.
);
```
The [`ConcordiumClient`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html) constructor also optionally takes a [`GrpcChannelOptions`](https://grpc.github.io/grpc/csharp-dotnet/api/Grpc.Net.Client.GrpcChannelOptions.html) object which can be used to specify various settings specific to the underlying [`GrpcChannel`](https://grpc.github.io/grpc/csharp-dotnet/api/Grpc.Net.Client.GrpcChannel.html) instance which handles the communication with the node. These could be settings that dictate the retry policy or specify parameters for the [keepalive ping](https://github.com/grpc/grpc/blob/master/doc/keepalive.md), which can be vital to the robustness of the application.

### Working with account transactions

Account transactions are blockchain transactions that are signed by and submitted on the behalf of an account. Classes relating to account transactions live in the [`Concordium.SDK.Transactions`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.html) namespace. All account transactions are modeled by records that inherit from the abstract record [`AccountTransactionPayload<T>`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.AccountTransactionPayload-1.html), where `T` is the type of the transaction. Inheriting records contain data specific to the transaction it models. One example of such is the [`Transfer`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.Transfer.html) record representing the transfer of a CCD amount from one account to another. It is instantiated as follows:

```csharp
using Concordium.Sdk.Types;
using Concordium.Sdk.Transactions;

CcdAmount amount = CcdAmount.FromCcd(100); // Send 100 CCD.
AccountAddress receiver = AccountAddress.From("4rvQePs6ZKFiW8rwY5nP18Uj2DroWiw9VPKTsTwfwmsjcFCJLy");
Transfer transfer = new Transfer(amount, receiver);
```

Since account transactions are submitted on behalf of an account, any [`AccountTransactionPayload<T>`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.AccountTransactionPayload-1.html) must be annotated with an [`AccountAddress`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.AccountAddress.html) of the account submitting the transaction, an account-specific [`AccountSequenceNumber`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.AccountSequenceNumber.html) (nonce) which is used to mitigate replay attacks and an [`Expiry`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.Expiry.html) representing a point in time after which the transaction will not be included in blocks whose (slot) time lies beyond it. The result of the annotation is a [`PreparedAccountTransaction<T>`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.PreparedAccountTransaction-1.html):

```csharp
AccountAddress sender = AccountAddress.From("3jfAuU1c4kPE6GkpfYw4KcgvJngkgpFrD9SkDBgFW3aHmVB5r1");
SequenceNumber sequenceNumber = Client.GetNextAccountSequenceNumber(sender).Item1;
Expiry expiry = Expiry.AtMinutesFromNow(10); // Transaction should expire 10 minutes after current system time.
PreparedAccountTransaction<Transfer> preparedTransfer = transfer.Prepare(sender, sequenceNumber, expiry);
```

Finally, the transaction must be signed using an [`ITransactionSigner`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.ITransactionSigner.html) which implements signing with the (secret) sign keys of the sender account, producing a [`SignedAccountTransaction<Transfer>`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.SignedAccountTransaction-1.html). In the following example the implementation used is the [`WalletAccount`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Wallets.WalletAccount.html) class which supports importing of account keys from the Concordium browser-wallet key export format:

```csharp
ITransactionSigner signer = WalletAccount.FromWalletKeyExportFormat("/path/to/exported-browser-wallet-keys.json");
Expiry expiry = Expiry.AtMinutesFromNow(10); // Transaction expires 10 minutes after the current system time.
SignedAccountTransaction<Transfer> signedTransfer = preparedTransfer.Sign(signer);
```

A signed transaction can be submitted to the blockchain by invoking the [`SendAccountTransaction`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html#Concordium_Sdk_Client_ConcordiumClient_SendAccountTransaction__1_Concordium_Sdk_Transactions_SignedAccountTransaction___0__) method. If the transfer was accepted by the node, its [`TransactionHash`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.TransactionHash.html) used to uniquely identify the transaction is returned: 

```csharp
TransactionHash = client.SendAccountTransaction(signedTransfer);
```

The [`TransactionHash`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.TransactionHash.html) can subsequently be used for querying the status of the transaction by invoking the [`GetBlockItemStatus`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.RawClient.html#Concordium_Sdk_Client_RawClient_GetBlockItemStatus_Concordium_Grpc_V2_TransactionHash_) method of the [`RawClient`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.RawClient.html), accessible through`client.Raw` in the above example. For more info on the raw API calls, see the [Using the raw client API](#using-the-raw-client-api) section.

### Working with transaction signers

As described in the [Account transactions](#working-with-account-transactions) section, account transactions must be signed using implementations of [`ITransactionSigner`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.ITransactionSigner.html). The SDK ships with the [`TransactionSigner`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Transactions.TransactionSigner.html) class which is dictionary based implementation to which [`ISigner`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Crypto.ISigner.html)s can be added. An [`ISigner`](developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Crypto.ISigner.html) represents a concrete implementation of a (secret) sign key for an account, and can be used to to write custom signer implementations which can be useful, for instance, if delegating the signing logic to a HSM. The SDK also ships with the [`WalletAccount`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Wallets.WalletAccount.html) class which provides functionality for importing account keys from one of the supported wallet export formats. Currently the Concordium browser and genesis wallet key export (JSON) formats are supported.

### Using the client API

A small subset of the raw methods generated from the Concordium Node gRPC API protocol buffer definitions have corresponding ergonomic wrappers that transparently map input and output types to and from their native equivalents as they are marshalled across the underlying generated API they wrap. One example of such is [`GetNextAccountSequenceNumber`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html#Concordium_Sdk_Client_ConcordiumClient_GetNextAccountSequenceNumber_Concordium_Sdk_Types_AccountAddress_) which takes an [`AccountAddress`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.AccountAddress.html) and returns a [`AccountSequenceNumber`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.AccountSequenceNumber.html):

```csharp
AccountAddress sender = AccountAddress.From("3jfAuU1c4kPE6GkpfYw4KcgvJngkgpFrD9SkDBgFW3aHmVB5r1");
AccountSequenceNumber sequenceNumber = client.GetNextAccountSequenceNumber(sender).Item1;
```

### Using the raw client API

The entire [Concordium Node gRPC API V2](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) is exposed through minimal wrappers of classes that model the interface types and services as they were generated from the protocol buffer schema definitions using the [`Grpc.Tools`](https://www.nuget.org/packages/Grpc.Tools/) and [`Grpc.Net.Client`](https://www.nuget.org/packages/Grpc.Net.Client) packages. These wrappers are defined in the [`RawClient`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.RawClient.html), instances of which can *only* be accessed through the [`Raw`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html#Concordium_Sdk_Client_ConcordiumClient_Raw) field of [`ConcordiumClient`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.ConcordiumClient.html) instances. 

As an example, the raw API call `GetAccountInfo` defined in the [Concordium Node gRPC API V2](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) takes as its input a gRPC message of the [AccountInfoRequest](https://developer.concordium.software/concordium-grpc-api/#concordium.v2.AccountInfoRequest) kind and expects a gRPC response of the [AccountInfo](https://developer.concordium.software/concordium-grpc-api/#concordium.v2.AccountInfo) kind. This method can be invoked through [`RawClient.GetAccountInfo`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Client.RawClient.html#Concordium_Sdk_Client_RawClient_GetAccountInfo_Concordium_Grpc_V2_AccountInfoRequest_) by supplying an instance of its corresponding generated type for [`AccountInfoRequest`](https://developer.concordium.software/concordium-grpc-api/#concordium.v2.AccountInfoRequest). In the following, we wish to retrieve the information of an account in the last finalized block:

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

Note that all generated types live in the [`Concordium.Grpc.V2`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Grpc.V2.html) namespace, and that there is a vast overlap between the names generated from the protocol buffer definitions file and the native types that live in the [`Concordium.Types`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.html) namespace. In the above we must therefore explicitly specify the namespace for [`Concordium.Sdk.Types.AccountAddress`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.AccountAddress.html) to resolve this ambiguity. Furthermore we leverage the convenience method [`Concordium.Sdk.Types.AccountAddress.ToAccountIdentifierInput`](http://developer.concordium.software/concordium-net-sdk/api/Concordium.Sdk.Types.AccountAddress.html#Concordium_Sdk_Types_AccountAddress_ToAccountIdentifierInput) to easily convert the the base58 address into its corresponding raw format. Also note that the overall structure of the interface types are one-to-one with the [Concordium gRPC API V2](https://github.com/Concordium/concordium-grpc-api/tree/main/v2/concordium), so we refer to its documentation and the [Runnable examples](#runnable-examples) section for more information on working with the types of the raw API.

## Runnable examples

A number of runnable examples illustrating usage of the SDK API are contained in the [examples](https://github.com/Concordium/concordium-net-sdk/main/examples) directory:

- [examples/RawClient](https://github.com/Concordium/concordium-net-sdk/main/examples/RawClient) demonstrates usage of the raw API methods exposed in `RawClient` (instances of which are accessible through the `Raw` field of the `ConcordiumClient`).
- [examples/Transactions](https://github.com/Concordium/concordium-net-sdk/main/examples/Transactions) demonstrates how to work with the various transaction types.

For instance, a runnable example akin to that given in [Working with account transactions](#working-with-account-transactions) is found in [examples/Transactions/Transfer](https://github.com/Concordium/concordium-net-sdk/main/examples/Transactions/Transfer). Compiling the project and running the resulting binary will print the following help message:

```text
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
                    gRPC V2 API is served.

  -p, --port        (Default: 20000) Port at the endpoint where the gRPC V2 API is served.

  -t, --timeout     (Default: 60) Default connection timeout in seconds.

  --help            Display this help screen.

  --version         Display version information.
```

To run the example with similar values, invoke the binary as follows:

```sh
$ Concordium.Sdk.Examples.Transactions.Transfer -a 100 -r 4rvQePs6ZKFiW8rwY5nP18Uj2DroWiw9VPKTsTwfwmsjcFCJLy -k /path/to/exported-browser-wallet-keys.json
```

Here, the sender account address is contained in the file specified by the `--keys` option which is why it is not included here. Upon successful submission of the transaction, the example program will print something like:

```text
Successfully submitted transfer transaction with hash 6bc9bfac5ef4aa1988ab8b1ab6007a736d4b3fe7e52b942d69a030319d979f13
```

## Documentation

Rendered documentation for this project is available [here](http://developer.concordium.software/concordium-net-sdk/).

## Migration

This deprecates earlier versions of the Concordium .NET SDK that used version 1 of the Concordium Node gRPC API. In terms of semantics and the information carried in messages, the APIs are quite similar, so APIs of the older SDK versions have corresponding raw methods in this version. Note that in some cases endpoints in the version 1 API are "split" into several endpoints in the version 2 API to increase the granularity.

Another major difference between this and the previous version of the SDK is that this version of the SDK currently does not support deploying contract modules with schemas.

## License

This project is licensed under the terms of the Mozilla Public License 2.0.

For more information, please refer to the [LICENSE](https://github.com/Concordium/concordium-net-sdk/blob/main/LICENSE) file.

## Contributing

Contributions are welcomed. Guidelines for contribution can be found [here](https://github.com/Concordium/.github/blob/main/.github/CODE_OF_CONDUCT.md). GitHub workflows specify CI jobs for building, unit testing and formatting. Passing build jobs is a requirement for a pull request to be considered eligible for merging. The formatting rules are specified in the [EditorConfig](https://editorconfig.org/) format and are found in the [.editorconfig](https://github.com/Concordium/concordium-net-sdk/blob/main/.editorconfig) file at the root of the project.

## Acknowledgements

This project is developed and maintained by the [Concordium Foundation](https://concordium.foundation/).
