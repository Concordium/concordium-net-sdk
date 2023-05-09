[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](https://github.com/Concordium/.github/blob/main/.github/CODE_OF_CONDUCT.md)
[![NuGet version](https://badge.fury.io/nu/ConcordiumNetSdk.svg)](https://badge.fury.io/nu/ConcordiumNetSdk)


# A .NET C# SDK for interacting with the Concordium blockchain

This SDK is a .NET integration library which adds support for constructing and sending various transactions, as well as querying various aspects of the Concordium blockchain and its nodes. The SDK uses version 2 of the [Concordium Node GRPC API](https://developer.concordium.software/concordium-grpc-api/#v2%2fconcordium%2fservice.proto) to interact with Concordium nodes and in turn the Concordium blockchain, and serves as a wrapper for this API with added ergonomics. Note that this deprecates earlier versions of the SDK that use version 1 of the API.

For more information, cfr. the [GitHub repository](https://github.com/Concordium/concordium-net-sdk) or the [rendered documentation](..).