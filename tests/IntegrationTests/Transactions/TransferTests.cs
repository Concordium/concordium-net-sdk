using Concordium.Sdk.Tests.IntegrationTests.Utils.LocalNode;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;
using FluentAssertions;

namespace Concordium.Sdk.Tests.IntegrationTests.Transactions;

[Trait("Category", "IntegrationLocalNodeTests")]
[Collection(LocalNodeCollectionFixture.LocalNodes)]
public sealed class TransferTests
{
    private readonly LocalNodeFixture _fixture;
    private readonly WalletAccount _account2;
    private readonly WalletAccount _account1;

    private const int Timeout = 120_000;

    public TransferTests(LocalNodeFixture fixture)
    {
        this._fixture = fixture;
        this._account1 = LocalNodeFixture.CreateWalletAccount(1);
        this._account2 = LocalNodeFixture.CreateWalletAccount(2);
    }

    [Fact(Timeout = Timeout)]
    public async Task WhenTransfer_ThenAmountTransferred()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);

        var accountInfoBefore_1 = await this._fixture.Client.GetAccountInfoAsync(this._account1.AccountAddress, new LastFinal(), cts.Token);
        var accountInfoBefore_2 = await this._fixture.Client.GetAccountInfoAsync(this._account2.AccountAddress, new LastFinal(), cts.Token);
        var amount = CcdAmount.FromCcd(42);

        var transfer = new Sdk.Transactions.Transfer(amount, this._account2.AccountAddress);
        var (sequenceNumber, _) = await this._fixture.Client.GetNextAccountSequenceNumberAsync(this._account1.AccountAddress, cts.Token);
        var preparedTransaction =
            transfer.Prepare(this._account1.AccountAddress, sequenceNumber, Expiry.AtMinutesFromNow(30));
        var signedTransfer = preparedTransaction.Sign(this._account1);
        var txHash = await this._fixture.Client.SendAccountTransactionAsync(signedTransfer, cts.Token);

        // Act
        var finalization = await TransactionTestHelpers.AwaitFinalization(txHash, this._fixture.Client, cts.Token);

        // Assert
        var accountTransfer = TransactionTestHelpers.ValidateAccountTransactionOutcome<AccountTransfer>(finalization);
        accountTransfer.Amount.Should().Be(amount);

        var accountInfoAfter_1 = await this._fixture.Client.GetAccountInfoAsync(this._account1.AccountAddress, new LastFinal(), cts.Token);
        var accountInfoAfter_2 = await this._fixture.Client.GetAccountInfoAsync(this._account2.AccountAddress, new LastFinal(), cts.Token);
        var details = finalization.State.Summary.Details as AccountTransactionDetails;

        (accountInfoBefore_1.Response.AccountAmount - amount - details!.Cost).Should().Be(accountInfoAfter_1.Response.AccountAmount);
        (accountInfoBefore_2.Response.AccountAmount + amount).Should().Be(accountInfoAfter_2.Response.AccountAmount);
    }
}
