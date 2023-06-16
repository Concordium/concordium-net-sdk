using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types;

/// <summary>
/// State of the passive delegation pool. Changes to delegation,
/// e.g., an account deciding to delegate are reflected in this structure at
/// first.
/// </summary>
/// <param name="DelegatedCapital">The total capital delegated passively.</param>
/// <param name="CommissionRates">The passive delegation commission rates.</param>
/// <param name="CurrentPaydayTransactionFeesEarned">The transaction fees accruing to the passive delegators in the current reward period.</param>
/// <param name="CurrentPaydayDelegatedCapital">The effective delegated capital to the passive delegators for the current reward period.</param>
/// <param name="AllPoolTotalCapital">Total capital staked across all pools, including passive delegation.</param>
public record PassiveDelegationStatus (
    CcdAmount DelegatedCapital,
    CommissionRates CommissionRates,
    CcdAmount CurrentPaydayTransactionFeesEarned,
    CcdAmount CurrentPaydayDelegatedCapital,
    CcdAmount AllPoolTotalCapital)
{
    internal static PassiveDelegationStatus From(PassiveDelegationInfo passiveDelegationInfoAsync) =>
        new(
            CcdAmount.From(passiveDelegationInfoAsync.DelegatedCapital),
            CommissionRates.From(passiveDelegationInfoAsync.CommissionRates),
            CcdAmount.From(passiveDelegationInfoAsync.CurrentPaydayTransactionFeesEarned),
            CcdAmount.From(passiveDelegationInfoAsync.CurrentPaydayDelegatedCapital),
            CcdAmount.From(passiveDelegationInfoAsync.AllPoolTotalCapital)
        );
}
