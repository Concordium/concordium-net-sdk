using System.Text;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;
using FluentAssertions;
using Xunit.Abstractions;
using static Concordium.Sdk.Tests.IntegrationTests.Transactions.TransactionTestHelpers;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;

namespace Concordium.Sdk.Tests.IntegrationTests.Types;

[Trait("Category", "IntegrationTests")]
[Collection("Using Wallet")]
public sealed class OnChainDataTests : Tests
{
    public OnChainDataTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact(Timeout = Timeout)]
    public async Task WhenTransferWithoutMemo_ThenNull()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);

        var filePath = this.GetString("walletPath");
        var walletData = await File.ReadAllTextAsync(filePath, cts.Token);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);
        var sender = account.AccountAddress;

        var to = this.GetString("transferTo");
        var receiver = AccountAddress.From(to);

        var transferPayload = new Transfer(CcdAmount.FromCcd(1), receiver);

        // Act
        var txHash = await this.Transfer(account, sender, transferPayload, cts.Token);
        var finalized = await this.AwaitFinalization(txHash, cts.Token);
        var transfer = ValidateAccountTransactionOutcome<AccountTransfer>(finalized);

        // Assert
        transfer.Memo.Should().BeNull();
    }

    [Fact(Timeout = Timeout)]
    public async Task GivenMemo_WhenTransfer_ThenMemoAbleToParse()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);

        var filePath = this.GetString("walletPath");
        var walletData = await File.ReadAllTextAsync(filePath, cts.Token);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);
        var sender = account.AccountAddress;

        var to = this.GetString("transferTo");
        var receiver = AccountAddress.From(to);

        const string expected = "fooBar";

        var memo = OnChainData.FromTextEncodeAsCBOR(expected);
        var transferPayload = new TransferWithMemo(CcdAmount.FromCcd(1), receiver, memo);

        var txHash = await this.Transfer(account, sender, transferPayload, cts.Token);
        var finalized = await this.AwaitFinalization(txHash, cts.Token);
        var transfer = ValidateAccountTransactionOutcome<AccountTransfer>(finalized);
        transfer!.Memo.Should().NotBeNull();

        // Act
        var cborDecodeToString = transfer.Memo!.TryCborDecodeToString();

        // Assert
        cborDecodeToString.Should().NotBeNull();
        cborDecodeToString.Should().Be(expected);
    }

    [Fact(Timeout = Timeout)]
    public async Task GivenUtf8Memo_WhenTransfer_ThenMemoPresent()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);
        var filePath = this.GetString("walletPath");
        var walletData = await File.ReadAllTextAsync(filePath, cts.Token);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);
        var sender = account.AccountAddress;

        var to = this.GetString("transferTo");
        var receiver = AccountAddress.From(to);

        const string expected = "foobar";

        var memo = OnChainData.From(Encoding.UTF8.GetBytes(expected));
        var transferPayload = new TransferWithMemo(CcdAmount.FromCcd(1), receiver, memo);

        var txHash = await this.Transfer(account, sender, transferPayload, cts.Token);
        var finalized = await this.AwaitFinalization(txHash, cts.Token);
        var transfer = ValidateAccountTransactionOutcome<AccountTransfer>(finalized);
        transfer!.Memo.Should().NotBeNull();

        // Act
        var utf8 = Encoding.UTF8.GetString(transfer.Memo!.AsSpan());

        // Assert
        utf8.Should().Be(expected);
    }
}
