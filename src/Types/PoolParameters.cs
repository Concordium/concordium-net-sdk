using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Parameters related to staking pools. This applies to protocol version 4 and
/// up.
/// </summary>
/// <param name="PassiveFinalizationCommission">Fraction of finalization rewards charged by the passive delegation.</param>
/// <param name="PassiveBakingCommission">Fraction of baking rewards charged by the passive delegation.</param>
/// <param name="PassiveTransactionCommission">Fraction of transaction rewards charged by the passive delegation</param>
/// <param name="CommissionBounds">Bounds on the commission rates that may be charged by bakers.</param>
/// <param name="MinimumEquityCapital">Minimum equity capital required for a new baker.</param>
/// <param name="CapitalBound">
/// Maximum fraction of the total staked capital of that a new baker can
/// have.
/// </param>
/// <param name="LeverageBound">
/// The maximum leverage that a baker can have as a ratio of total stake
/// to equity capital.
/// </param>
public sealed record PoolParameters(
    AmountFraction PassiveFinalizationCommission,
    AmountFraction PassiveBakingCommission,
    AmountFraction PassiveTransactionCommission,
    CommissionRanges CommissionBounds,
    CcdAmount MinimumEquityCapital,
    CapitalBound CapitalBound,
    LeverageFactor LeverageBound
)
{
    internal static PoolParameters From(Grpc.V2.PoolParametersCpv1 pool) =>
        new(
            AmountFraction.From(pool.PassiveFinalizationCommission),
            AmountFraction.From(pool.PassiveBakingCommission),
            AmountFraction.From(pool.PassiveTransactionCommission),
            CommissionRanges.From(pool.CommissionBounds),
            pool.MinimumEquityCapital.ToCcd(),
            CapitalBound.From(pool.CapitalBound),
            LeverageFactor.From(pool.LeverageBound));
}
