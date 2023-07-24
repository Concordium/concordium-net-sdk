using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;
using FluentAssertions;
using Xunit.Abstractions;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;
using AccountTransactionDetails = Concordium.Sdk.Types.AccountTransactionDetails;

namespace Concordium.Sdk.Tests.IntegrationTests.Types;

[Trait("Category", "IntegrationTests")]
public sealed class OnChainDataTests : Tests
{
    public OnChainDataTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact(Timeout = 30_000)]
    public async Task WhenTransferWithoutMemo_ThenNull()
    {
        // Arrange
        using var cts = new CancellationTokenSource(30_000);

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
        var transfer = ValidateArrangementOutcome(finalized);

        // Assert
        transfer.Memo.Should().BeNull();
    }


    [Fact(Timeout = 60_000)]
    public async Task GivenMemo_WhenTransfer_ThenMemoAbleToParse()
    {
        // Arrange
        using var cts = new CancellationTokenSource(60_000);

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
        var transfer = ValidateArrangementOutcome(finalized);
        transfer!.Memo.Should().NotBeNull();

        // Act
        var cborDecodeToString = transfer.Memo!.TryCborDecodeToString();

        // Assert
        cborDecodeToString.Should().NotBeNull();
        cborDecodeToString.Should().Be(expected);
    }

    private static AccountTransfer ValidateArrangementOutcome(TransactionStatusFinalized finalized)
    {
        finalized.State.Summary.Details.Should().BeOfType<AccountTransactionDetails>();
        var details = finalized.State.Summary.Details as AccountTransactionDetails;
        details!.Effects.Should().BeOfType<AccountTransfer>();
        var transfer = details.Effects as AccountTransfer;
        return transfer!;
    }
}
