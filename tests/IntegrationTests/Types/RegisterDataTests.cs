using System.Text;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;
using FluentAssertions;
using Xunit.Abstractions;
using static Concordium.Sdk.Tests.IntegrationTests.Transactions.TransactionTestHelpers;

namespace Concordium.Sdk.Tests.IntegrationTests.Types;

[Trait("Category", "IntegrationTests")]
[Collection("Using Wallet")]
public sealed class RegisterDataTests : Tests
{
    public RegisterDataTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact(Timeout = Timeout)]
    public async Task GivenCborData_WhenRegisterData_ThenAbleToParse()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);

        var filePath = this.GetString("walletPath");
        var walletData = await File.ReadAllTextAsync(filePath, cts.Token);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);
        var sender = account.AccountAddress;

        const string expected = "fooBar";

        var memo = OnChainData.FromTextEncodeAsCBOR(expected);
        var registerData = new RegisterData(memo);

        var txHash = await this.Transfer(account, sender, registerData, cts.Token);
        var finalized = await this.AwaitFinalization(txHash, cts.Token);
        var registeredData = ValidateAccountTransactionOutcome<DataRegistered>(finalized);

        // Act
        var cborDecodeToString = OnChainData.From(registeredData.Data).TryCborDecodeToString();

        // Assert
        cborDecodeToString.Should().NotBeNull();
        cborDecodeToString.Should().Be(expected);
    }

    [Fact(Timeout = Timeout)]
    public async Task GivenUtf8Data_WhenRegisterData_ThenAbleToParse()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);

        var filePath = this.GetString("walletPath");
        var walletData = await File.ReadAllTextAsync(filePath, cts.Token);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);
        var sender = account.AccountAddress;

        const string expected = "fooBar";

        var memo = OnChainData.From(Encoding.UTF8.GetBytes(expected));
        var registerData = new RegisterData(memo);

        var txHash = await this.Transfer(account, sender, registerData, cts.Token);
        var finalized = await this.AwaitFinalization(txHash, cts.Token);
        var registeredData = ValidateAccountTransactionOutcome<DataRegistered>(finalized);

        // Act
        var cborDecodeToString = Encoding.UTF8.GetString(registeredData.Data);

        // Assert
        cborDecodeToString.Should().NotBeNull();
        cborDecodeToString.Should().Be(expected);
    }
}
