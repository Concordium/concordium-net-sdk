using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// An individual release of a locked balance.
/// </summary>
/// <param name="Timestamp">Effective time of the release.</param>
/// <param name="Amount">Amount to be released.</param>
/// <param name="Transactions">List of transaction hashes that contribute a balance to this release.</param>
public sealed record Release(
    DateTimeOffset Timestamp,
    CcdAmount Amount,
    IList<TransactionHash> Transactions
)
{
    internal static Release From(Grpc.V2.Release release) => new(
        release.Timestamp.ToDateTimeOffset(),
        CcdAmount.From(release.Amount),
        release.Transactions.Select(TransactionHash.From).ToList()
    );
}
