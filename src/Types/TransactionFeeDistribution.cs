namespace Concordium.Sdk.Types;

/// <summary>
/// Update the transaction fee distribution to the specified value.
/// </summary>
/// <param name="Baker">The fraction that goes to the baker of the block.</param>
/// <param name="GasAccount">
/// The fraction that goes to the gas account. The remaining fraction will
/// go to the foundation.
/// </param>
public sealed record TransactionFeeDistribution(AmountFraction Baker, AmountFraction GasAccount)
{
    internal static TransactionFeeDistribution From(Grpc.V2.TransactionFeeDistribution distribution) =>
        new(
            AmountFraction.From(distribution.Baker),
            AmountFraction.From(distribution.GasAccount)
        );
}
