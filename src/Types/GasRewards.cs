namespace Concordium.Sdk.Types;

/// <summary>
/// The reward fractions related to the gas account and inclusion of special
/// transactions for chain parameters version 0 and 1.
/// </summary>
/// <param name="Baker">Fraction of the previous gas account paid to the baker.</param>
/// <param name="FinalizationProof">Fraction paid for including a finalization proof in a block.</param>
/// <param name="AccountCreation">Fraction paid for including each account creation transaction in a block.</param>
/// <param name="ChainUpdate">Fraction paid for including an update transaction in a block.</param>
public sealed record GasRewards(AmountFraction Baker, AmountFraction FinalizationProof, AmountFraction AccountCreation, AmountFraction ChainUpdate)
{
    internal static GasRewards From(Grpc.V2.GasRewards rewards) =>
        new(
            AmountFraction.From(rewards.Baker),
            AmountFraction.From(rewards.FinalizationProof),
            AmountFraction.From(rewards.AccountCreation),
            AmountFraction.From(rewards.ChainUpdate)
        );
}

/// <summary>
/// The reward fractions related to the gas account and inclusion of special
/// transactions.
/// Introduce for protocol version 6.
/// </summary>
/// <param name="Baker">Fraction of the previous gas account paid to the baker.</param>
/// <param name="AccountCreation">Fraction paid for including each account creation transaction in a block.</param>
/// <param name="ChainUpdate">Fraction paid for including an update transaction in a block.</param>
public sealed record GasRewardsCpv2(AmountFraction Baker, AmountFraction AccountCreation, AmountFraction ChainUpdate)
{
    internal static GasRewardsCpv2 From(Grpc.V2.GasRewardsCpv2 rewards) =>
        new(
            AmountFraction.From(rewards.Baker),
            AmountFraction.From(rewards.AccountCreation),
            AmountFraction.From(rewards.ChainUpdate)
        );
}
