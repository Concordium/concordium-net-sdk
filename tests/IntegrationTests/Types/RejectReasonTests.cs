using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;
using FluentAssertions;
using Xunit.Abstractions;

namespace Concordium.Sdk.Tests.IntegrationTests.Types;

[Trait("Category", "IntegrationTests")]
[Collection("Using Wallet")]
public sealed class RejectReasonTests : Tests
{
    public RejectReasonTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact(Timeout = Timeout)]
    public async Task WhenTransferWithInsufficientAmount_ThenReject()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);

        var filePath = this.GetString("walletPath");
        var walletData = await File.ReadAllTextAsync(filePath, cts.Token);
        var account = WalletAccount.FromWalletKeyExportFormat(walletData);
        var sender = account.AccountAddress;

        var to = this.GetString("transferTo");
        var receiver = AccountAddress.From(to);

        var toBigTransfer = new Transfer(CcdAmount.FromMicroCcd(ulong.MaxValue), receiver);

        // Act
        var txHash = await this.Transfer(account, sender, toBigTransfer, cts.Token);
        var finalized = await this.AwaitFinalization(txHash, cts.Token);

        // Assert
        finalized.State.Summary.Details.Should().BeOfType<AccountTransactionDetails>();
        var details = finalized.State.Summary.Details as AccountTransactionDetails;
        details!.Effects.Should().BeOfType<None>();
        var none = details.Effects as None;
        none!.RejectReason.Should().BeOfType<AmountTooLarge>();
        var amountToLarge = none.RejectReason as AmountTooLarge;
        amountToLarge!.Address.Should().BeOfType<AccountAddress>();
    }
}
