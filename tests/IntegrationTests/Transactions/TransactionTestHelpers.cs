using Concordium.Sdk.Client;
using Concordium.Sdk.Types;
using FluentAssertions;

namespace Concordium.Sdk.Tests.IntegrationTests.Transactions;

internal static class TransactionTestHelpers
{
    internal static T ValidateAccountTransactionOutcome<T>(TransactionStatusFinalized finalized) where T : class
    {
        finalized.State.Summary.Details.Should().BeOfType<AccountTransactionDetails>();
        var details = finalized.State.Summary.Details as AccountTransactionDetails;
        details!.Effects.Should().BeOfType<T>();
        var transfer = details.Effects as T;
        return transfer!;
    }

    internal static async Task<TransactionStatusFinalized> AwaitFinalization(TransactionHash txHash, ConcordiumClient client, CancellationToken token)
    {
        while (true)
        {
            if (!token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            var transactionStatus = await client.GetBlockItemStatusAsync(txHash, token);

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
}
