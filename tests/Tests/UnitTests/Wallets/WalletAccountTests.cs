using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;
using Concordium.Sdk.Crypto;
using System;
using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.UnitTests.Transactions;

public class WalletAccountTests
{
    [Theory]
    [WalletDataAttribute(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/GenesisWalletKeyExportFormatValid.json",
        0,
        0,
        "443c20439711361b6870c1679be33860d10cf7cded240e4a567e31ec3a56ecf5"
    )]
    [WalletDataAttribute(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/GenesisWalletKeyExportFormatValidNonZeroIndices.json",
        17,
        37,
        "443c20439711361b6870c1679be33860d10cf7cded240e4a567e31ec3a56ecf5"
    )]
    [WalletDataAttribute(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/BrowserWalletKeyExportFormatValid.json",
        0,
        0,
        "56f60de843790c308dac2d59a5eec9f6b1649513f827e5a13d7038accfe31784"
    )]
    [WalletDataAttribute(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/BrowserWalletKeyExportFormatValidNonZeroIndices.json",
        17,
        37,
        "56f60de843790c308dac2d59a5eec9f6b1649513f827e5a13d7038accfe31784"
    )]
    public void FromWalletKeyExportFormat_OnValidInput_ReturnsCorrectInstance(
        string walletJson,
        byte credIndex,
        byte keyIndex,
        string expectedKey
    )
    {
        WalletAccount wallet = WalletAccount.FromWalletKeyExportFormat(walletJson);

        // Get the sign key at the specified account credential index, key index pair.
        ImmutableDictionary<AccountKeyIndex, ISigner>? keys;
        wallet.GetSignerEntries().TryGetValue(credIndex, out keys);

        if (keys == null)
        {
            throw new ArgumentException($"No sign keys with account credential index {credIndex}.");
        }

        ISigner? key;
        keys.TryGetValue(keyIndex, out key);

        if (key == null)
        {
            throw new ArgumentException(
                $"No sign keys with account credential index {credIndex} and key index {keyIndex}."
            );
        }

        if (key.GetType() != typeof(Ed25519SignKey))
        {
            throw new ArgumentException(
                $"Sign key should be of type {typeof(Ed25519SignKey).ToString()}, but got {key.GetType().ToString()} instead."
            );
        }

        Ed25519SignKey signKey = (Ed25519SignKey)key;

        key.ToString().Should().BeEquivalentTo(expectedKey);
        wallet.GetSignerEntries().Count.Should().Be(1);
    }

    [Theory]
    [EmbeddedResourceData(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/GenesisWalletKeyExportFormatMissingField.json"
    )]
    [EmbeddedResourceData(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/BrowserWalletKeyExportFormatMissingField.json"
    )]
    public void FromWalletKeyExportFormat_OnInputWithMissingField_ThrowsException(string json)
    {
        Action result = () => WalletAccount.FromWalletKeyExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [EmbeddedResourceData(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/GenesisWalletKeyExportFormatInvalidKeyIndex.json"
    )]
    [EmbeddedResourceData(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/GenesisWalletKeyExportFormatInvalidKeyIndex.json"
    )]
    public void FromWalletKeyExportFormat_OnInputWithInvalidKeyIndex_ThrowsException(string json)
    {
        Action result = () => WalletAccount.FromWalletKeyExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [EmbeddedResourceData(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/GenesisWalletKeyExportFormatInvalidCredentialIndex.json"
    )]
    [EmbeddedResourceData(
        "Concordium.Sdk.Tests/UnitTests/Wallets/Data/BrowserWalletKeyExportFormatInvalidCredentialIndex.json"
    )]
    public void FromWalletKeyExportFormat_OnInputWithInvalidCredentialIndex_ThrowsException(
        string json
    )
    {
        Action result = () => WalletAccount.FromWalletKeyExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Fact]
    public void FromWalletKeyExportFormat_OnNonJsonInput_ThrowsException()
    {
        Action result = () => WalletAccount.FromWalletKeyExportFormat("Not JSON.");
        result.Should().Throw<Newtonsoft.Json.JsonException>();
    }
}
