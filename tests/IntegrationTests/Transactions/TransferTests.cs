using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using Concordium.Sdk.Wallets;
using FluentAssertions;

namespace Concordium.Sdk.Tests.IntegrationTests.Transactions;

public sealed class TransferTests : IDisposable
{
    private readonly ConcordiumClient _client;
    private readonly WalletAccount _account2;
    private readonly WalletAccount _account1;

    private const int Timeout = 120_000;

    public TransferTests()
    {
        this._account1 = CreateWalletAccount(1);
        this._account2 = CreateWalletAccount(2);
        this._client = new ConcordiumClient(new Uri("http://localhost:20100"), new ConcordiumClientOptions());
    }

    [Fact(Timeout = Timeout)]
    public async Task WhenTransfer_ThenAmountTransferred()
    {
        // Arrange
        using var cts = new CancellationTokenSource(Timeout);

        var accountInfoBefore_1 = await this._client.GetAccountInfoAsync(this._account1.AccountAddress, new LastFinal(), cts.Token);
        var accountInfoBefore_2 = await this._client.GetAccountInfoAsync(this._account2.AccountAddress, new LastFinal(), cts.Token);
        var amount = CcdAmount.FromCcd(42);

        var transfer = new Sdk.Transactions.Transfer(amount, this._account2.AccountAddress);
        var (sequenceNumber, _) = await this._client.GetNextAccountSequenceNumberAsync(this._account1.AccountAddress, cts.Token);
        var preparedTransaction =
            transfer.Prepare(this._account1.AccountAddress, sequenceNumber, Expiry.AtMinutesFromNow(30));
        var signedTransfer = preparedTransaction.Sign(this._account1);
        var txHash = await this._client.SendAccountTransactionAsync(signedTransfer, cts.Token);

        // Act
        var finalization = await TransactionTestHelpers.AwaitFinalization(txHash, this._client, cts.Token);

        // Assert
        var accountTransfer = TransactionTestHelpers.ValidateAccountTransactionOutcome<AccountTransfer>(finalization);
        accountTransfer.Amount.Should().Be(amount);

        var accountInfoAfter_1 = await this._client.GetAccountInfoAsync(this._account1.AccountAddress, new LastFinal(), cts.Token);
        var accountInfoAfter_2 = await this._client.GetAccountInfoAsync(this._account2.AccountAddress, new LastFinal(), cts.Token);
        var details = finalization.State.Summary.Details as AccountTransactionDetails;

        (accountInfoBefore_1.Response.AccountAmount - amount - details!.Cost).Should().Be(accountInfoAfter_1.Response.AccountAmount);
        (accountInfoBefore_2.Response.AccountAmount + amount).Should().Be(accountInfoAfter_2.Response.AccountAmount);
    }

    private static WalletAccount CreateWalletAccount(int id)
    {
        var path = File.ReadAllText($"./local/accounts/stagenet-{id+1}.json");
        var account = WalletAccount.FromWalletKeyExportFormat(path);
        return account!;
    }

    public void Dispose() => this._client.Dispose();
}
