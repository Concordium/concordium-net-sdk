using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Wallets;
using ConcordiumNetSdk.Crypto;
using System;
using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class WalletAccountTests
{
    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/GenesisWalletKeyExportFormatValid.json"
    )]
    public void FromGenesisWalletExportFormat_OnValidInput_ReturnsCorrectInstance(string json)
    {
        WalletAccount genesisWallet = WalletAccount.FromGenesisWalletExportFormat(json);

        // Get the sign key at account credential index 0, key index 0.
        ImmutableDictionary<AccountKeyIndex, ISigner>? keys;
        genesisWallet.GetSignerEntries().TryGetValue(0, out keys);

        if (keys == null)
        {
            throw new ArgumentException($"No sign keys with account credential index 0.");
        }

        ISigner? key;
        keys.TryGetValue(0, out key);

        if (key == null)
        {
            throw new ArgumentException(
                $"No sign keys with account credential index 0 and key index 0."
            );
        }

        if (key.GetType() != typeof(Ed25519SignKey))
        {
            throw new ArgumentException(
                $"Sign key should be of type {typeof(Ed25519SignKey).ToString()}, but got {key.GetType().ToString()} instead."
            );
        }

        Ed25519SignKey signKey = (Ed25519SignKey)key;

        key.ToString()
            .Should()
            .BeEquivalentTo("443c20439711361b6870c1679be33860d10cf7cded240e4a567e31ec3a56ecf5");
        genesisWallet.GetSignerEntries().Count.Should().Be(1);
    }

    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/GenesisWalletKeyExportFormatMissingField.json"
    )]
    public void FromGenesisWalletExportFormat_OnInputWithMissingField_ThrowsException(string json)
    {
        Action result = () => WalletAccount.FromGenesisWalletExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/GenesisWalletKeyExportFormatInvalidKeyIndex.json"
    )]
    public void FromGenesisWalletExportFormat_OnInputWithInvalidKeyIndex_ThrowsException(
        string json
    )
    {
        Action result = () => WalletAccount.FromGenesisWalletExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/GenesisWalletKeyExportFormatInvalidCredentialIndex.json"
    )]
    public void FromGenesisWalletExportFormat_OnInputWithInvalidCredentialIndex_ThrowsException(
        string json
    )
    {
        Action result = () => WalletAccount.FromGenesisWalletExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Fact]
    public void FromGenesisWalletExportFormat_OnNonJsonInput_ThrowsException()
    {
        Action result = () => WalletAccount.FromGenesisWalletExportFormat("nice {} gobbeldygook");
        result.Should().Throw<Newtonsoft.Json.JsonException>();
    }

    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/BrowserWalletKeyExportFormatValid.json"
    )]
    public void FromBrowserWalletExportFormat_OnValidInput_ReturnsCorrectInstance(string json)
    {
        WalletAccount genesisWallet = WalletAccount.FromBrowserWalletExportFormat(json);

        // Get the sign key at account credential index 0, key index 0.
        ImmutableDictionary<AccountKeyIndex, ISigner>? keys;
        genesisWallet.GetSignerEntries().TryGetValue(0, out keys);

        if (keys == null)
        {
            throw new ArgumentException($"No sign keys with account credential index 0.");
        }

        ISigner? key;
        keys.TryGetValue(0, out key);

        if (key == null)
        {
            throw new ArgumentException(
                $"No sign keys with account credential index 0 and key index 0."
            );
        }

        if (key.GetType() != typeof(Ed25519SignKey))
        {
            throw new ArgumentException(
                $"Sign key should be of type {typeof(Ed25519SignKey).ToString()}, but got {key.GetType().ToString()} instead."
            );
        }

        Ed25519SignKey signKey = (Ed25519SignKey)key;
        key.ToString()
            .Should()
            .BeEquivalentTo("56f60de843790c308dac2d59a5eec9f6b1649513f827e5a13d7038accfe31784");
        genesisWallet.GetSignerEntries().Count.Should().Be(1);
    }

    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/BrowserWalletKeyExportFormatMissingField.json"
    )]
    public void FromBrowserWalletExportFormat_OnInputWithMissingField_ThrowsException(string json)
    {
        Action result = () => WalletAccount.FromBrowserWalletExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/BrowserWalletKeyExportFormatInvalidKeyIndex.json"
    )]
    public void FromBrowserWalletExportFormat_OnInputWithInvalidKeyIndex_ThrowsException(
        string json
    )
    {
        Action result = () => WalletAccount.FromBrowserWalletExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [EmbeddedResourceData(
        "ConcordiumNetSdk.UnitTests/Wallets/Data/BrowserWalletKeyExportFormatInvalidCredentialIndex.json"
    )]
    public void FromBrowserWalletExportFormat_OnInputWithInvalidCredentialIndex_ThrowsException(
        string json
    )
    {
        Action result = () => WalletAccount.FromBrowserWalletExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Fact]
    public void FromBrowserWalletExportFormat_OnNonJsonInput_ThrowsException()
    {
        Action result = () => WalletAccount.FromBrowserWalletExportFormat("nice {} gobbeldygook");
        result.Should().Throw<Newtonsoft.Json.JsonException>();
    }
}
