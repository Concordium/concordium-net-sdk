namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Details of a party in a finalization.
/// </summary>
/// <param name="BakerId">The identity of the baker.</param>
/// <param name="Weight">The party's relative weight in the committee</param>
/// <param name="Signed">Whether the party's signature is present</param>
public record FinalizationSummaryParty(BakerId BakerId, ulong Weight, bool Signed)
{
    internal static FinalizationSummaryParty From(Grpc.V2.FinalizationSummaryParty party) =>
        new(
            new BakerId(new AccountIndex(party.Baker.Value)),
            party.Weight,
            party.Signed
        );
}
