using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types.Baker;

/// <summary>
/// Data contained in the transaction response in case a baker stake was updated
/// (either increased or decreased.)
/// </summary>
/// <param name="BakerId">Affected baker.</param>
/// <param name="NewStake">New stake.</param>
/// <param name="Increased">
/// A boolean which indicates whether it increased
/// (`true`) or decreased (`false`).
/// </param>
public record BakerStakeUpdatedData(BakerId BakerId, CcdAmount NewStake, bool Increased)
{
    internal static BakerStakeUpdatedData? From(Grpc.V2.BakerStakeUpdatedData? data)
    {
        if (data == null)
        {
            return null;
        }

        return new BakerStakeUpdatedData(
            BakerId.From(data.BakerId),
            data.NewStake.ToCcd(),
            data.Increased
        );
    }
}
