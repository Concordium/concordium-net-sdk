using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using Concordium.Sdk.Client;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Xunit.Abstractions;

namespace Concordium.Sdk.Tests.IntegrationTests;

[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
public abstract class Tests : IDisposable
{
    private readonly JsonDocument _json;

    protected ITestOutputHelper Output { get; }
    protected ConcordiumClient Client { get; }

    protected Tests(ITestOutputHelper output)
    {
        this.Output = output;

        var assemblyPath = typeof(Tests).GetTypeInfo().Assembly.Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
        var jsonPath = Path.Combine(assemblyDirectory!, "test_configuration.json");
        this._json = JsonDocument.Parse(File.ReadAllText(jsonPath));

        var uri = this.GetString("uri");

        this.Client = new ConcordiumClient(new Uri(uri), new ConcordiumClientOptions());
    }

    protected async Task<TransactionStatusFinalized> AwaitFinalization(TransactionHash txHash, CancellationToken token)
    {
        while (true)
        {
            if (!token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            var transactionStatus = await this.Client.GetBlockItemStatusAsync(txHash, token);

            switch (transactionStatus)
            {
                case TransactionStatusFinalized transactionStatusFinalized:
                    return transactionStatusFinalized;
                default:
                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                    break;
            }
        }
    }

    protected async Task<TransactionHash> Transfer(ITransactionSigner account, AccountAddress sender, AccountTransactionPayload transactionPayload, CancellationToken token)
    {
        var (accountSequenceNumber, _) = await this.Client.GetNextAccountSequenceNumberAsync(sender, token);
        var preparedAccountTransaction = transactionPayload.Prepare(sender, accountSequenceNumber, Expiry.AtMinutesFromNow(30));
        var signedTransfer = preparedAccountTransaction.Sign(account);
        var txHash = await this.Client.SendAccountTransactionAsync(signedTransfer, token);
        return txHash;
    }

    protected string GetString(string name) => this.GetConfiguration(name).GetString()!;

    private JsonElement GetConfiguration(string name)
    {
        var jsonElement = this._json.RootElement.GetProperty(name);
        return jsonElement;
    }

    public void Dispose()
    {
        this._json.Dispose();
        this.Client.Dispose();
    }
}
