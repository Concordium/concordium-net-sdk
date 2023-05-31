using System;
using System.IO;
using Concordium.Sdk.Crypto;
using Concordium.Sdk.Wallets;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Wallets;

public class WalletAccountTests
{
    [Theory]
    [InlineData("./data/wallets/GenesisWalletKeyExportFormatValid.json", 0, 0, "443c20439711361b6870c1679be33860d10cf7cded240e4a567e31ec3a56ecf5")]
    [InlineData("./data/wallets/GenesisWalletKeyExportFormatValidNonZeroIndices.json", 17, 37, "443c20439711361b6870c1679be33860d10cf7cded240e4a567e31ec3a56ecf5")]
    [InlineData("./data/wallets/BrowserWalletKeyExportFormatValid.json", 0, 0, "56f60de843790c308dac2d59a5eec9f6b1649513f827e5a13d7038accfe31784")]
    [InlineData("./data/wallets/BrowserWalletKeyExportFormatValidNonZeroIndices.json", 17, 37, "56f60de843790c308dac2d59a5eec9f6b1649513f827e5a13d7038accfe31784")]
    public void FromWalletKeyExportFormat_OnValidInput_ReturnsCorrectInstance(
        string path,
        byte credIndex,
        byte keyIndex,
        string expectedKey
    )
    {
        var json = File.ReadAllText(path);
        var wallet = WalletAccount.FromWalletKeyExportFormat(json);

        // Get the sign key at the specified account credential index, key index pair.
        wallet.GetSignerEntries().TryGetValue(credIndex, out var keys);

        if (keys == null)
        {
            throw new ArgumentException($"No sign keys with account credential index {credIndex}.");
        }

        keys.TryGetValue(keyIndex, out var key);

        if (key == null)
        {
            throw new ArgumentException(
                $"No sign keys with account credential index {credIndex} and key index {keyIndex}."
            );
        }

        if (key.GetType() != typeof(Ed25519SignKey))
        {
            throw new ArgumentException(
                $"Sign key should be of type {typeof(Ed25519SignKey)}, but got {key.GetType()} instead."
            );
        }

        key.ToString().Should().BeEquivalentTo(expectedKey);
        wallet.GetSignerEntries().Count.Should().Be(1);
    }

    [Theory]
    [InlineData("./data/wallets/GenesisWalletKeyExportFormatInvalidMissingField.json")]
    [InlineData("./data/wallets/BrowserWalletKeyExportFormatInvalidMissingField.json")]
    public void FromWalletKeyExportFormat_OnInputWithMissingField_ThrowsException(string path)
    {
        var json = File.ReadAllText(path);
        Action result = () => WalletAccount.FromWalletKeyExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [InlineData("./data/wallets/GenesisWalletKeyExportFormatInvalidKey.json")]
    [InlineData("./data/wallets/BrowserWalletKeyExportFormatInvalidKey.json")]
    public void FromWalletKeyExportFormat_OnInputWithInvalidKey_ThrowsException(string path)
    {
        var json = File.ReadAllText(path);
        Action result = () => WalletAccount.FromWalletKeyExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [InlineData("./data/wallets/GenesisWalletKeyExportFormatInvalidKeyIndex.json")]
    [InlineData("./data/wallets/BrowserWalletKeyExportFormatInvalidKeyIndex.json")]
    public void FromWalletKeyExportFormat_OnInputWithInvalidKeyIndex_ThrowsException(string path)
    {
        var json = File.ReadAllText(path);
        Action result = () => WalletAccount.FromWalletKeyExportFormat(json);
        result.Should().Throw<WalletDataSourceException>();
    }

    [Theory]
    [InlineData("./data/wallets/GenesisWalletKeyExportFormatInvalidCredentialIndex.json")]
    [InlineData("./data/wallets/BrowserWalletKeyExportFormatInvalidCredentialIndex.json")]
    public void FromWalletKeyExportFormat_OnInputWithInvalidCredentialIndex_ThrowsException(
        string path
    )
    {
        var json = File.ReadAllText(path);
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
